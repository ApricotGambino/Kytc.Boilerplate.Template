//namespace Api.UnitTests.ConfigurationTests;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Domain.Entities.Admin;
//using Domain.Entities.Common;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;
//using TestShared.Fixtures;

//[Category(TestingCategoryConstants.ApiStartupTests)]
//public class asdfasdfasdf : BaseTestFixture
//{

//    //TODO: Implement a keyset pagination as well.
//    private List<Log> Getlogs()
//    {
//        var list = new List<Log>();
//        list.Add(new Log() { Id = 1, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 1", TimeStamp = DateTime.Now.AddDays(-30) });
//        list.Add(new Log() { Id = 2, Level = "Fatal", MessageTemplate = "MessageTemplate", Message = "Test log 2", TimeStamp = DateTime.Now.AddDays(-15) });
//        list.Add(new Log() { Id = 3, Level = "Warning", MessageTemplate = "MessageTemplate", Message = "Test log 3", TimeStamp = DateTime.Now.AddDays(-2) });
//        list.Add(new Log() { Id = 4, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 4", TimeStamp = DateTime.Now });
//        return list;
//    }

//    private static List<T> GetLogsT<T>()
//    {
//        //NOTE: We're doing this as a generic method to represent how this would really be used, 
//        //since we're very unlikely to paginate a hardened list. But if we are, then we can do something like this
//        //to make the generic pagination methods work. 
//        return
//        [
//            (T)(object)new Log() { Id = 1, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 1", TimeStamp = DateTime.Now.AddDays(-30) },
//            (T)(object)new Log() { Id = 2, Level = "Fatal", MessageTemplate = "MessageTemplate", Message = "Test log 2", TimeStamp = DateTime.Now.AddDays(-10) },
//            (T)(object)new Log() { Id = 3, Level = "Warning", MessageTemplate = "MessageTemplate", Message = "Test log 3", TimeStamp = DateTime.Now.AddDays(-9) },
//            (T)(object)new Log() { Id = 4, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 4", TimeStamp = DateTime.Now.AddDays(-8) },
//            (T)(object)new Log() { Id = 5, Level = "Warning", MessageTemplate = "MessageTemplate", Message = "Test log 5", TimeStamp = DateTime.Now.AddDays(-7) },
//            (T)(object)new Log() { Id = 6, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 6", TimeStamp = DateTime.Now.AddDays(-6) },
//            (T)(object)new Log() { Id = 7, Level = "Fatal", MessageTemplate = "MessageTemplate", Message = "Test log 7", TimeStamp = DateTime.Now.AddDays(-5) },
//            (T)(object)new Log() { Id = 8, Level = "Warning", MessageTemplate = "MessageTemplate", Message = "Test log 8", TimeStamp = DateTime.Now.AddDays(-4) },
//            (T)(object)new Log() { Id = 9, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 9", TimeStamp = DateTime.Now.AddDays(-3) },
//            (T)(object)new Log() { Id = 10, Level = "Fatal", MessageTemplate = "MessageTemplate", Message = "Test log 10", TimeStamp = DateTime.Now.AddDays(-2) },
//            (T)(object)new Log() { Id = 11, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 11", TimeStamp = DateTime.Now.AddDays(-1) }
//        ];
//    }

//    private static List<Log> GetLogs()
//    {
//        //NOTE: We're doing this as a generic method to represent how this would really be used, 
//        //since we're very unlikely to paginate a hardened list. But if we are, then we can do something like this
//        //to make the generic pagination methods work. 
//        var dateTime = DateTime.Now;
//        return
//        [
//            new Log() { Id = 1, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 1", TimeStamp = dateTime.AddDays(-30) },
//            new Log() { Id = 2, Level = "Fatal", MessageTemplate = "MessageTemplate", Message = "Test log 2", TimeStamp = dateTime.AddDays(-10) },
//            new Log() { Id = 3, Level = "Warning", MessageTemplate = "MessageTemplate", Message = "Test log 3", TimeStamp = dateTime.AddDays(-9) },
//            new Log() { Id = 4, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 4", TimeStamp = dateTime.AddDays(-8) },
//            new Log() { Id = 5, Level = "Warning", MessageTemplate = "MessageTemplate", Message = "Test log 5", TimeStamp = dateTime.AddDays(-7) },
//            new Log() { Id = 6, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 6", TimeStamp = dateTime.AddDays(-6) },
//            new Log() { Id = 7, Level = "Fatal", MessageTemplate = "MessageTemplate", Message = "Test log 7", TimeStamp = dateTime.AddDays(-5) },
//            new Log() { Id = 8, Level = "Warning", MessageTemplate = "MessageTemplate", Message = "Test log 8", TimeStamp = dateTime.AddDays(-4) },
//            new Log() { Id = 9, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 9", TimeStamp = dateTime.AddDays(-3) },
//            new Log() { Id = 10, Level = "Fatal", MessageTemplate = "MessageTemplate", Message = "Test log 10", TimeStamp = dateTime.AddDays(-2) },
//            new Log() { Id = 11, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 11", TimeStamp = dateTime.AddDays(-1) }
//        ];
//    }

//    private List<Log> GetLogsOffsetPagination(int pageNumber = 1, int pageSize = 10, string orderByProperty = nameof(BaseEntity.CreatedDate), bool orderByAscending = false)
//    {
//        var result = Getlogs().AsQueryable();

//        Func<Log, Object> orderByPropertyFunc = s => s.GetType().GetProperty(orderByProperty.Trim()).GetValue(s);

//        if (orderByAscending)
//        {
//            result = result.OrderBy(orderByPropertyFunc).AsQueryable();
//            var b = 1;
//        }
//        else
//        {
//            result = result.OrderByDescending(orderByPropertyFunc).AsQueryable();
//        }

//        return result
//        .Skip((pageNumber - 1) * pageSize)
//        .Take(pageSize)
//        .ToList();
//    }
//    private List<Log> GetLogsKeysetPagination(int pageNumber = 1, int pageSize = 10, string orderByProperty = nameof(BaseEntity.CreatedDate), bool orderByAscending = false)
//    {
//        //Keyset Pagination always requires a unique ordering. 
//        //If you're ordering by a Date, there COULD be multiple records with that date, and 
//        //keyset pagination could miss some records, so we need to use a composite key for ordering.
//        //In the date example, we can use Date + ID to make a unique ordering.

//        //Keyset pagination fixes offset pagination's performace issues:
//        //In offset pagination, if we skip/take, the db still has to process rows prior to the skip. 
//        //Offset pagination also isn't 'page stable', as if any updates happen, those COULD be skipped or duplicate them. 

//        //More info on why using Keyset is a good idea here: 
//        //https://learn.microsoft.com/en-us/ef/core/querying/pagination

//        var lastDate = new DateTime(2020, 1, 1);
//        var lastId = 55;
//        //var nextPage = await context.Posts
//        //    .OrderBy(b => b.Date)
//        //    .ThenBy(b => b.PostId)
//        //    .Where(b => b.Date > lastDate || (b.Date == lastDate && b.PostId > lastId))
//        //    .Take(10)
//        //    .ToListAsync();
//        var result = Getlogs().AsQueryable();

//        Func<Log, Object> orderByPropertyFunc = s => s.GetType().GetProperty(orderByProperty.Trim()).GetValue(s);
//        Func<Log, Object> whereFunction = s => (DateTime)s.GetType().GetProperty(orderByProperty.Trim()).GetValue(s) > lastDate;

//        if (orderByAscending)
//        {
//            result = result.OrderBy(orderByPropertyFunc).ThenBy(o => o.Id).AsQueryable();
//            var b = 1;
//        }
//        else
//        {
//            result = result.OrderByDescending(orderByPropertyFunc).ThenByDescending(o => o.Id).AsQueryable();
//        }

//        return result
//        //.Where(whereFunction)
//        ////.Where(b => b.Date > lastDate || (b.Date == lastDate && b.PostId > lastId))
//        .Take(pageSize)
//        .ToList();
//    }


//    private List<T> GetItems<T, TKey>(Func<T, TKey> keySelector, TKey value)
//    {
//        var result = GetLogsT<T>();
//        //result = result
//        //  .Where(t => keySelector(t).Equals(value))
//        //  .ToList();
//        return result.ToList();

//    }

//    //private List<T> GetLogsKeysetPaginationTest<T, TKey>(Func<T, TKey> keySelector, TKey value)
//    //{
//    //    //var nextPage = await context.Posts
//    //    //    .OrderBy(b => b.Date)
//    //    //    .ThenBy(b => b.PostId)
//    //    //    .Where(b => b.Date > lastDate || (b.Date == lastDate && b.PostId > lastId))
//    //    //    .Take(10)
//    //    //    .ToListAsync();

//    //    var result = GetlogsT<T>()
//    //      //.Where(t => keySelector(t) > value)
//    //      .ToList();
//    //    return result.ToList();

//    //}


//    //[Test]
//    //public async Task GenericTest()
//    //{
//    //    var thing = GetLogsKeysetPaginationTest<Log, int>(item => item.Id, 2);
//    //    var a = 1;
//    //}

//    private List<T> GetKeysetPaginationTest1<T, TKey>(Func<T, TKey> keySelector, TKey value, int lastId, int pageSize)
//        where T : BaseEntity
//        where TKey : IComparable
//    {
//        var thing = GetLogs()
//            .Where(p => keySelector(p as T).CompareTo(value) > 0 || (keySelector(p as T).CompareTo(value) == 0 && p.Id > lastId))
//          .Take(pageSize)
//          .ToList();

//        var result = GetLogsT<T>()
//          //.Where(t => keySelector(t).CompareTo(value) < 0)//negative = before, zero = this one, positive = next one
//          .Where(p => keySelector(p).CompareTo(value) > 0 || (keySelector(p).CompareTo(value) == 0 && p.Id > lastId))
//          .Take(pageSize)
//          .ToList();
//        return result.ToList();
//    }

//    [Test]
//    public async Task KeySetPaginationTests()
//    {
//        const int pageSize = 3;
//        var allLogs = GetLogs();

//        //var oldestFirstSetOf3Logs = allLogs.OrderBy(o => o.TimeStamp).Skip(0).Take(pageSize).ToList();
//        //var oldestSecondSetOf3Logs = allLogs.OrderBy(o => o.TimeStamp).Skip(pageSize).Take(pageSize).ToList();
//        //var oldestThirdSetOf3Logs = allLogs.OrderBy(o => o.TimeStamp).Skip(pageSize * 2).Take(pageSize).ToList();
//        //var oldestFourthAndFinalSetOf2Logs = allLogs.OrderBy(o => o.TimeStamp).Skip(pageSize * 3).Take(pageSize).ToList();

//        //var newest3Logs = allLogs.OrderByDescending(o => o.TimeStamp).Take(pageSize).ToList();
//        //var secondNewest3Logs = allLogs.OrderByDescending(o => o.TimeStamp).Skip(pageSize).Take(pageSize).ToList();
//        //var exactMiddleLog = secondOldest3Logs[^1] == secondNewest3Logs[^1] ? secondOldest3Logs[^1] : null;

//        //Get first page order by oldest timestamp first.
//        using (Assert.EnterMultipleScope())
//        {
//            //Arrange
//            var expectedRecordCount = allLogs.Count >= pageSize ? pageSize : allLogs.Count;
//            var expectedOldestFirstPageOfRecords = allLogs.OrderBy(o => o.TimeStamp).Take(pageSize).ToList();


//            //Act
//            var firstKeySetPaginatedPage = allLogs.KeysetPagination(item => item.TimeStamp, DateTime.MinValue, int.MinValue, pageSize: pageSize);

//            //Assert
//            Assert.That(expectedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedRecordCount));
//            Assert.That(expectedOldestFirstPageOfRecords, Is.Ordered.By(nameof(Log.TimeStamp)));



//            Assert.That(firstKeySetPaginatedPage, Has.Count.EqualTo(expectedRecordCount));
//            Assert.That(oldest3Logs.ConvertAll(s => s.Id), Is.EqualTo(firstKeySetPaginatedPage.ConvertAll(s => s.Id)).AsCollection);
//        }

//        //Get next page order by oldest timestamp first. 
//        var lastRecordFromPreviousKeySetPage = firstKeySetPaginatedPage[^1];
//        var secondKeySetPaginatedPage = allLogs.KeysetPagination(item => item.TimeStamp, lastRecordFromPreviousKeySetPage.TimeStamp, lastRecordFromPreviousKeySetPage.Id, pageSize: pageSize);
//        using (Assert.EnterMultipleScope())
//        {
//            Assert.That(secondKeySetPaginatedPage, Has.Count.EqualTo(pageSize));
//            Assert.That(secondOldest3Logs, Is.Ordered.By(nameof(Log.TimeStamp)));
//            Assert.That(secondKeySetPaginatedPage.ConvertAll(s => s.Id), Is.EqualTo(secondOldest3Logs.ConvertAll(s => s.Id)).AsCollection);
//        }

//        //Get next page order by oldest timestamp first. 
//        lastRecordFromPreviousKeySetPage = secondKeySetPaginatedPage[^1];
//        var thirdKeySetPaginatedPage = allLogs.KeysetPagination(item => item.TimeStamp, lastRecordFromPreviousKeySetPage.TimeStamp, lastRecordFromPreviousKeySetPage.Id, pageSize: pageSize);
//        using (Assert.EnterMultipleScope())
//        {
//            Assert.That(thirdKeySetPaginatedPage, Has.Count.EqualTo(pageSize));
//            Assert.That(thirdKeySetPaginatedPage.ConvertAll(s => s.Id), Is.EqualTo(secondNewest3Logs.ConvertAll(s => s.Id)).AsCollection);
//        }


//        var lastDateTime = DateTime.Now.AddDays(-15);
//        var lastId = 2;
//        var anotherthing = GetLogs().KeysetPagination<Log, DateTime>(item => item.TimeStamp, lastDateTime, 2, 10);
//        //var thing = GetKeysetPaginationTest1<Log, DateTime>(item => item.TimeStamp, lastDateTime, 2, 10);
//        var a = 1;
//    }
//    //Guid value = ...
//    //var items = GetItems<MyClass, Guid>(item => item.ParentId, value);
//    [Test]
//    public async Task OffSetPaginationTest()
//    {
//        var logCount = GetLogsOffsetPagination(1, 2, nameof(Log.TimeStamp));
//        Assert.That(logCount, Has.Count.EqualTo(2));

//        var logCountDescending = GetLogsOffsetPagination(1, 2, nameof(Log.TimeStamp));
//        Assert.That(logCountDescending, Is.Ordered.Descending.By(nameof(Log.TimeStamp)));

//        //var allLogs = Getlogs();
//        var logCountAscending = GetLogsOffsetPagination(1, 2, nameof(Log.TimeStamp), true);
//        Assert.That(logCountAscending, Is.Ordered.Ascending.By(nameof(Log.TimeStamp)));
//    }



//    [Test]
//    public async Task GetAllWithOffsetPagination()
//    {
//        //Replace with the name of the test method.
//        //EX: GetUserRoles_UserHasNoRoles_ReturnsEmptyListOfRoles
//        //EX: GetUserRoles_UserIsAdmin_ReturnsListThatContainsAdminRole
//        //EX: GetUserRoles_UserDoesNotExist_ThrowsUserNotFoundException

//        //Arrange
//        //EX: userService.Add(new User { Name = "Test User", Roles = ["Admin"] });

//        //Act
//        //EX: var result = await userService.GetUserRoles("Test User");

//        //Assert
//        //EX: Assert.Contains("Admin", result);
//    }

//    [Test]
//    public async Task GetAllWithKeysetPagination()
//    {
//        //Replace with the name of the test method.
//        //EX: GetUserRoles_UserHasNoRoles_ReturnsEmptyListOfRoles
//        //EX: GetUserRoles_UserIsAdmin_ReturnsListThatContainsAdminRole
//        //EX: GetUserRoles_UserDoesNotExist_ThrowsUserNotFoundException

//        //Arrange
//        //EX: userService.Add(new User { Name = "Test User", Roles = ["Admin"] });

//        //Act
//        //EX: var result = await userService.GetUserRoles("Test User");

//        //Assert
//        //EX: Assert.Contains("Admin", result);
//    }

//    [Test]
//    public async Task GetAllWithoutPagination()
//    {
//        //Replace with the name of the test method.
//        //EX: GetUserRoles_UserHasNoRoles_ReturnsEmptyListOfRoles
//        //EX: GetUserRoles_UserIsAdmin_ReturnsListThatContainsAdminRole
//        //EX: GetUserRoles_UserDoesNotExist_ThrowsUserNotFoundException

//        //Arrange
//        //EX: userService.Add(new User { Name = "Test User", Roles = ["Admin"] });

//        //Act
//        //EX: var result = await userService.GetUserRoles("Test User");

//        //Assert
//        //EX: Assert.Contains("Admin", result);
//    }
//}

////public static class MyExtensions
////{
////    public static List<T> KeysetPagination<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey lastValue, int lastId, int pageSize)
////        where T : BaseEntity
////        where TKey : IComparable
////    {
////        //NOTE: This method is designed to be used after an initial Offset pagination query, or at least an already ordered data set.
////        //Additionally, this method is only used when you're fetching data by pageSize either forward or backward one page at a time.
////        //This method is more efficient navigating pages of data than using .Skip().Take() every time which offset pagination requires.

////        //NOTE: You probably don't want to use this on it's own.
////        //This method only works if you know where you were, and you're iterating up or down only one page at a time.
////        //But if that's the situation you're in (which is most common), then getting your dataset with Offset pagination, 
////        //and using this keyset pagination method to get subsequent pages is going to be more efficient than constantly using offset pagination.

////        var result = list
////          //.Where(t => keySelector(t).CompareTo(value) < 0)//negative = before, zero = this one, positive = next one
////          .Where(p => keySelector(p).CompareTo(lastValue) > 0 || (keySelector(p).CompareTo(lastValue) == 0 && p.Id > lastId))
////          .Take(pageSize)
////          .ToList();
////        return result.ToList();
////    }
////}