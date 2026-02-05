//namespace Domain.Extensions;

//using System;
//using System.Collections.Generic;
//using Domain.Entities.Admin;
//using Domain.Entities.Common;
//using Microsoft.EntityFrameworkCore;

////You should use OffsetPagination when you're first ordering a list.
////You should use KeysetPagination after OffsetPagination, and you're going through the pages of data.
////KeysetPagination is much more efficient than calling OffsetPagination over and over increasing the skip().

////EX: A user clicks 'sort' on an 'IsActive' table header, wanting to sort the records by those that are, and are not 'IsActive' as a boolean.
////We expect to return the first 10 records to the UI ordered by IsActive (true or false, don't care, just one or the other) 
//// First, call the OffSetPagination(o => o.IsActive, 10)
////In the UI, store the last ID and value of the last record,
////then when the user clicks 'next' on the pages of data, call GetNextPageUsingKeysetPagination
////if the the user clicks 'back' on the pages of data, call GetPreviousPageUsingKeysetPagination

////DO NOT call keysetPagination when you don't know what the last page was. 
////EX: A user is on page 1, but clicks page 3, unless you know the last record of page 3. use offset, because we're not 'moving forward or backwards by one page', so calling 'GetNext' won't work.


////You SHOULD NOT assume ordering by offset and keyset will yield the same results, as
////they probably won't.

////A good example of this would be ordering by a boolean.
////Offset uses linq's orderby() with take() and skip()
////so if you order by A boolean, you're going get a list with all the 'false' values first, then all the 'true' values,
////because false = 0, true = 1.
////Let's assume we have a list equalPartsTrueAndFalse that looks like this:
////[{id:1,false}, {id:2,true}, {id:3,false}, {id:4,true}, {id:5,false}, {id:6,true}]
////EX: Offset basically does this: equalPartsTrueAndFalse.orderBy(o => o.ABool).thenBy(id)
////Which returns:
//// [1-false, 3-false, 5-false, 2-true, 4-true, 6-true]
////If we wanted 'true' values first, then we'd actually have to say equalPartsTrueAndFalse.orderByDescending(o => o.ABool)
//// [2-true, 4-true, 6-true, 1-false, 3-false, 5-false,]

////Keyset uses linq's orderby as well, but it also considers the last value being searched, along with the ID,
////so it would orderBy where the boolean value.
////If we wanted 'true' values first, we actually would do this: equalPartsTrueAndFalse.Where(p => p.ABool >= true)
////which would only return: [ 2-true, 4-true, 6-true]
////And you can see that removes all 'false' values, and will always be ordered by ID. Which is why Keyset also uses the ID.
////So it ACTUALLY does something like this: equalPartsTrueAndFalse.Where(p => p.ABool >= true).OrderedById().ThenIncludeTheResultsYouFilteredOut().OrderThoseFilteredOutResultsById()
////Which makes it now return:
////[ 2-true, 4-true, 6-true, 1-false, 3-false, 5-false]


////Now, let's talk about WHY we do this complicated dance.
////Again, assume we have this list: equalPartsTrueAndFalse = [1-false, 2-true, 3-false, 4-true, 5-false, 6-true]
////and we will fetch data 2 records at a time, orderd by ABool, with 'true' first (meaning descending).

////Let's look at this from only using offset:
////firstPage = equalPartsTrueAndFalse.OffsetPaginationDescending(o => o.ABool, pageNumber: 1, pageSize: 2);
////[1-false, 2-true]
////nextPage = equalPartsTrueAndFalse.OffsetPaginationDescending(o => o.ABool, pageNumber: 2, pageSize: 2);
////[6-true, 1-false]
////nextPage = equalPartsTrueAndFalse.OffsetPaginationDescending(o => o.ABool, pageNumber: 3, pageSize: 2);
////3-false, 5-false]
////Because OffsetPagination uses .Skip() and .Take(), we actually had to iterate over EVERY record in the dataset both times, then discarded unwanted results.
////This is an oversimplificiation, but once you start to consider millions of records, you can see how this could be better.

////Let's look at this from only using keyset:
////firstPage = equalPartsTrueAndFalse.GetNextPageUsingKeysetPagination(o => o.ABool, lastValue: "true", id: 0, 2);
////[1-false, 2-true]
////nextPage = equalPartsTrueAndFalse.GetNextPageUsingKeysetPagination(o => o.ABool, lastValue: firstPage.Last().ABool, id: firstPage.Last().Id, 2);
////[6-true, 1-false]
////nextPage = equalPartsTrueAndFalse.GetNextPageUsingKeysetPagination(o => o.ABool, lastValue: nextPage.Last().ABool, id: nextPage.Last().Id, 2);
////3-false, 5-false]
////So this works, but you can see we had to 'know' the last value, and the last id to make this work, and since this is almost certainly
////being used from a user in the UI, we can't just 'store' the previous values in C#. (I mean..Without machine state garbage, that's nasty anyway)
////This is a lot more efficient though, because we dont' have to scan EVERY record, because this will skip any records with an ID less than what we're lookign for.

////Finally, let's look at how keyset and offset work together.
////firstPage = equalPartsTrueAndFalse.OffsetPaginationDescending(o => o.ABool, pageNumber: 1, pageSize: 2);
////[1-false, 2-true]
////nextPage = equalPartsTrueAndFalse.GetNextPageUsingKeysetPagination(o => o.ABool, lastValue: firstPage.Last().ABool, id: firstPage.Last().Id, 2);
////[6-true, 1-false]
////nextPage = equalPartsTrueAndFalse.GetNextPageUsingKeysetPagination(o => o.ABool, lastValue: nextPage.Last().ABool, id: nextPage.Last().Id, 2);
////3-false, 5-false]
////So this code looks similar to the keyset example except in the first case we use the offset, but with this you can have two methods that fetch data for sorting.
////In the UI, when a user clicks sort on a column, we return data using Offset slow, but works without additional information.
////Then if the user clicks 'next', we can pass back the last value from the offset get into a keyset API method since we now have more information (last value,id)
////If the user changes the order to be sorted, or jumps pages, then we have to call offset again. 
//public static class PaginationExtensions
//{


//    public static List<T> OffsetPaginationDBSETTEST<T, TKey>(this DbSet<T> list, Func<T, TKey> keySelector, int pageNumber, int pageSize)
//        where T : BaseEntity
//        where TKey : IComparable
//    {
//        //NOTE: Pagenumber is not an index, and therefore doesn't start at 0
//        return list
//          .OrderBy(keySelector)
//          .ThenBy(p => p.Id)
//          .Skip((pageNumber - 1) * pageSize)
//          .Take(pageSize)
//          .ToList();
//    }

//    public static List<T> GetNextPageUsingKeysetPaginationDBSETTEST<T, TKey>(this DbSet<T> list, Func<T, TKey> keySelector, TKey lastValue, int lastId, int pageSize)
//        where T : BaseEntity
//        where TKey : IComparable
//    {
//        //NOTE: This method is designed to be used after an initial Offset pagination query, or at least an already ordered data set.
//        //Additionally, this method is only used when you're fetching data by pageSize either forward or backward one page at a time.
//        //This method is more efficient navigating pages of data than using .Skip().Take() every time which offset pagination requires.

//        //NOTE: You probably don't want to use this on it's own.
//        //This method only works if you know where you were, and you're iterating up or down only one page at a time.
//        //But if that's the situation you're in (which is most common), then getting your dataset with Offset pagination, 
//        //and using this keyset pagination method to get subsequent pages is going to be more efficient than constantly using offset pagination.
//        return list
//          //.Where(p => (keySelector(p).CompareTo(lastValue) == 0 && p.Id > lastId) || keySelector(p).CompareTo(lastValue) > 0)
//          .Where(p => (keySelector(p).CompareTo(lastValue) == 0 && p.Id > lastId) || keySelector(p).CompareTo(lastValue) > 0)
//          .OrderBy(keySelector)
//          .Take(pageSize)
//          .ToList();
//    }

//    public static List<T> GetNextPageUsingKeysetPaginationasdfasdfasdf<T, TKey>(this DbSet<T> list, Func<T, TKey> keySelector, int lastValue, int lastId, int pageSize)
//        where T : Log
//        where TKey : IComparable
//    {
//        //NOTE: This method is designed to be used after an initial Offset pagination query, or at least an already ordered data set.
//        //Additionally, this method is only used when you're fetching data by pageSize either forward or backward one page at a time.
//        //This method is more efficient navigating pages of data than using .Skip().Take() every time which offset pagination requires.

//        //NOTE: You probably don't want to use this on it's own.
//        //This method only works if you know where you were, and you're iterating up or down only one page at a time.
//        //But if that's the situation you're in (which is most common), then getting your dataset with Offset pagination, 
//        //and using this keyset pagination method to get subsequent pages is going to be more efficient than constantly using offset pagination.
//        return list
//          //.Where(p => (keySelector(p).CompareTo(lastValue) == 0 && p.Id > lastId) || keySelector(p).CompareTo(lastValue) > 0)
//          //.Where(p => p.GetType().GetProperty(lastValue.ToString()).GetValue(p) == p.GetType().GetProperty(lastValue.ToString()).GetValue(p))
//          .Where(p => (p.Id == lastValue && p.Id > lastId) || p.Id > lastValue)
//          .OrderBy(keySelector)
//          .Take(pageSize)
//          .ToList();
//    }












//    /// <summary>
//    /// This function performs pagination ordering by keyset, and should be used only when you're paging through
//    /// data first ordered by using the offset pagination method. Do not use this method as a starting point for ordering,
//    /// as the results will likely not match expectations. 
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <typeparam name="TKey"></typeparam>
//    /// <param name="list"></param>
//    /// <param name="keySelector"></param>
//    /// <param name="lastValue"></param>
//    /// <param name="lastId"></param>
//    /// <param name="pageSize"></param>
//    /// <returns></returns>
//    public static List<T> GetNextPageUsingKeysetPagination<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey lastValue, int lastId, int pageSize)
//        where T : BaseEntity
//        where TKey : IComparable
//    {
//        //NOTE: This method is designed to be used after an initial Offset pagination query, or at least an already ordered data set.
//        //Additionally, this method is only used when you're fetching data by pageSize either forward or backward one page at a time.
//        //This method is more efficient navigating pages of data than using .Skip().Take() every time which offset pagination requires.

//        //NOTE: You probably don't want to use this on it's own.
//        //This method only works if you know where you were, and you're iterating up or down only one page at a time.
//        //But if that's the situation you're in (which is most common), then getting your dataset with Offset pagination, 
//        //and using this keyset pagination method to get subsequent pages is going to be more efficient than constantly using offset pagination.
//        return list
//          .Where(p => (keySelector(p).CompareTo(lastValue) == 0 && p.Id > lastId) || keySelector(p).CompareTo(lastValue) > 0)
//          .OrderBy(keySelector)
//          .Take(pageSize)
//          .ToList();
//    }

//    //TODO: Test this.
//    public static List<T> GetPreviousPageUsingKeysetPagination<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey lastValue, int lastId, int pageSize)
//       where T : BaseEntity
//       where TKey : IComparable
//    {
//        //NOTE: This method is designed to be used after an initial Offset pagination query, or at least an already ordered data set.
//        //Additionally, this method is only used when you're fetching data by pageSize either forward or backward one page at a time.
//        //This method is more efficient navigating pages of data than using .Skip().Take() every time which offset pagination requires.

//        //NOTE: You probably don't want to use this on it's own.
//        //This method only works if you know where you were, and you're iterating up or down only one page at a time.
//        //But if that's the situation you're in (which is most common), then getting your dataset with Offset pagination, 
//        //and using this keyset pagination method to get subsequent pages is going to be more efficient than constantly using offset pagination.
//        return list
//          .Where(p => (keySelector(p).CompareTo(lastValue) == 0 && p.Id < lastId) || keySelector(p).CompareTo(lastValue) < 0)
//          .OrderBy(keySelector)
//          .Take(pageSize)
//          .ToList();
//    }

//    /// <summary>
//    /// This function performs pagination ordering by offset, and should be used as your initial ordering point.
//    /// afterwards, you should use the Keyset pagination provided your order doesn't change, as it is more efficient
//    /// than using offset pagination to iterate pages.
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <typeparam name="TKey"></typeparam>
//    /// <param name="list"></param>
//    /// <param name="keySelector"></param>
//    /// <param name="pageNumber"></param>
//    /// <param name="pageSize"></param>
//    /// <returns></returns>
//    public static List<T> OffsetPagination<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, int pageNumber, int pageSize)
//        where T : BaseEntity
//        where TKey : IComparable
//    {
//        //NOTE: Pagenumber is not an index, and therefore doesn't start at 0
//        return list
//          .OrderBy(keySelector)
//            .ThenBy(p => p.Id)
//            .Skip((pageNumber - 1) * pageSize)
//            .Take(pageSize)
//            .ToList();
//    }

//    //TODO: Test this.
//    public static List<T> OffsetPaginationDescending<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, int pageNumber, int pageSize)
//        where T : BaseEntity
//        where TKey : IComparable
//    {
//        //NOTE: Pagenumber is not an index, and therefore doesn't start at 0
//        return list
//          .OrderByDescending(keySelector)
//            .ThenBy(p => p.Id)
//            .Skip((pageNumber - 1) * pageSize)
//            .Take(pageSize)
//            .ToList();
//    }
//}
