namespace Domain.Extensions;

using System;
using System.Collections.Generic;
using Domain.Entities.Common;

public static class PaginationExtensions
{
    public static List<T> KeysetPagination<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey lastValue, int lastId, int pageSize)
        where T : BaseEntity
        where TKey : IComparable
    {
        //NOTE: This method is designed to be used after an initial Offset pagination query, or at least an already ordered data set.
        //Additionally, this method is only used when you're fetching data by pageSize either forward or backward one page at a time.
        //This method is more efficient navigating pages of data than using .Skip().Take() every time which offset pagination requires.

        //NOTE: You probably don't want to use this on it's own.
        //This method only works if you know where you were, and you're iterating up or down only one page at a time.
        //But if that's the situation you're in (which is most common), then getting your dataset with Offset pagination, 
        //and using this keyset pagination method to get subsequent pages is going to be more efficient than constantly using offset pagination.

        var result = list
          .Where(p => (keySelector(p).CompareTo(lastValue) == 0 && p.Id > lastId) || keySelector(p).CompareTo(lastValue) > 0)
          .OrderBy(keySelector)
          .Take(pageSize)
          .ToList();
        return result.ToList();
    }


}