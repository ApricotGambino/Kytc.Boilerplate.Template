
namespace Domain.UnitTests.ExtensionTests.PaginationTests;

using System.Linq;
using NUnit.Framework;

using TestShared.TestObjects;

//using Domain.Extensions.List;

public class KeysetPaginationTests : PaginationTestFixture
{
    [Test]
    public async Task KeySetPagination_GetFirstPageOfDataOrderedByOldestFirst_ResultsMatchKnownOrder()
    {
        //Arrange
        var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDate).Take(_pageSize).ToList();

        //Act
        var keysetPaginatedOldestFirstPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.CreatedDate, DateTime.MinValue, int.MinValue, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestFirstPageOfRecords, Has.Count.EqualTo(_pageSize));
            Assert.That(expectedOldestFirstPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDate)));
            Assert.That(keysetPaginatedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedOldestFirstPageOfRecords.Count));
            Assert.That(expectedOldestFirstPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(keysetPaginatedOldestFirstPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }
    [Test]
    public async Task KeySetPagination_GetSecondPageOfDataOrderedByOldestFirst_ResultsMatchKnownOrder()
    {
        //Arrange
        var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDate).Take(_pageSize).ToList();
        var expectedOldestSecondPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDate).Skip(_pageSize).Take(_pageSize).ToList();
        var expectedLastRecordFromOldestFirstPageOfRecords = expectedOldestFirstPageOfRecords.Last();

        //Act
        var keysetPaginatedOldestSecondPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.CreatedDate, expectedLastRecordFromOldestFirstPageOfRecords.CreatedDate, expectedLastRecordFromOldestFirstPageOfRecords.Id, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestSecondPageOfRecords, Has.Count.EqualTo(_pageSize));
            Assert.That(expectedOldestSecondPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDate)));
            Assert.That(keysetPaginatedOldestSecondPageOfRecords, Has.Count.EqualTo(expectedOldestSecondPageOfRecords.Count));
            Assert.That(expectedOldestSecondPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(keysetPaginatedOldestSecondPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }
    [Test]
    public async Task KeySetPagination_GetLastPageOfOrderedDataByOldestFirst_ResultsMatchKnownOrder()
    {
        //Arrange
        var expectedOldestLastPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDate).TakeLast(_pageSize).ToList();
        var expectedOldestRecordBeforeLastPage = _paginationTestRecords.OrderBy(o => o.CreatedDate).TakeLast(_pageSize + 1).First();

        //Act
        var keysetPaginatedOldestLastPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.CreatedDate, expectedOldestRecordBeforeLastPage.CreatedDate, expectedOldestRecordBeforeLastPage.Id, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestLastPageOfRecords, Has.Count.EqualTo(_pageSize));
            Assert.That(expectedOldestLastPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDate)));
            Assert.That(keysetPaginatedOldestLastPageOfRecords, Has.Count.EqualTo(expectedOldestLastPageOfRecords.Count));
            Assert.That(keysetPaginatedOldestLastPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(expectedOldestLastPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }

    [Test]
    public async Task KeySetPagination_PaginateForwardByOldestWithPageSizeGreaterThanRemainingData_ReturnsASmallerPage()
    {
        //Arrange
        var oldestLastPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDate).TakeLast(_pageSize).ToList();
        var expectedOldestRecordInLastPageOfRecords = oldestLastPageOfRecords.First();
        var expectedOldestLastPageOfRecords = oldestLastPageOfRecords.OrderBy(o => o.CreatedDate).Skip(1).ToList();

        //Act
        var keysetPaginatedOldestLastPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.CreatedDate, expectedOldestRecordInLastPageOfRecords.CreatedDate, expectedOldestRecordInLastPageOfRecords.Id, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestLastPageOfRecords, Has.Count.EqualTo(_pageSize - 1));
            Assert.That(expectedOldestLastPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDate)));
            Assert.That(keysetPaginatedOldestLastPageOfRecords, Has.Count.EqualTo(expectedOldestLastPageOfRecords.Count));
            Assert.That(keysetPaginatedOldestLastPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(expectedOldestLastPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }

    ////const int pageSize = 3;
    ////var allLogs = GetLogs();

    ////var oldestFirstSetOf3Logs = allLogs.OrderBy(o => o.TimeStamp).Skip(0).Take(pageSize).ToList();
    ////var oldestSecondSetOf3Logs = allLogs.OrderBy(o => o.TimeStamp).Skip(pageSize).Take(pageSize).ToList();
    ////var oldestThirdSetOf3Logs = allLogs.OrderBy(o => o.TimeStamp).Skip(pageSize * 2).Take(pageSize).ToList();
    ////var oldestFourthAndFinalSetOf2Logs = allLogs.OrderBy(o => o.TimeStamp).Skip(pageSize * 3).Take(pageSize).ToList();

    ////var newest3Logs = allLogs.OrderByDescending(o => o.TimeStamp).Take(pageSize).ToList();
    ////var secondNewest3Logs = allLogs.OrderByDescending(o => o.TimeStamp).Skip(pageSize).Take(pageSize).ToList();
    ////var exactMiddleLog = secondOldest3Logs[^1] == secondNewest3Logs[^1] ? secondOldest3Logs[^1] : null;

    ////Get first page order by oldest timestamp first.
    //using (Assert.EnterMultipleScope())
    //{
    //    //Arrange
    //    var expectedRecordCount = _numberPaginationRecords.Count >= pageSize ? pageSize : allLogs.Count;
    //    var expectedOldestFirstPageOfRecords = allLogs.OrderBy(o => o.TimeStamp).Take(pageSize).ToList();


    //    //Act
    //    var firstKeySetPaginatedPage = allLogs.KeysetPagination(item => item.TimeStamp, DateTime.MinValue, int.MinValue, pageSize: pageSize);

    //    //Assert
    //    Assert.That(expectedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedRecordCount));
    //    Assert.That(expectedOldestFirstPageOfRecords, Is.Ordered.By(nameof(Log.TimeStamp)));



    //    Assert.That(firstKeySetPaginatedPage, Has.Count.EqualTo(expectedRecordCount));
    //    Assert.That(oldest3Logs.ConvertAll(s => s.Id), Is.EqualTo(firstKeySetPaginatedPage.ConvertAll(s => s.Id)).AsCollection);
    //}

    ////Get next page order by oldest timestamp first. 
    //var lastRecordFromPreviousKeySetPage = firstKeySetPaginatedPage[^1];
    //var secondKeySetPaginatedPage = allLogs.KeysetPagination(item => item.TimeStamp, lastRecordFromPreviousKeySetPage.TimeStamp, lastRecordFromPreviousKeySetPage.Id, pageSize: pageSize);
    //using (Assert.EnterMultipleScope())
    //{
    //    Assert.That(secondKeySetPaginatedPage, Has.Count.EqualTo(pageSize));
    //    Assert.That(secondOldest3Logs, Is.Ordered.By(nameof(Log.TimeStamp)));
    //    Assert.That(secondKeySetPaginatedPage.ConvertAll(s => s.Id), Is.EqualTo(secondOldest3Logs.ConvertAll(s => s.Id)).AsCollection);
    //}

    ////Get next page order by oldest timestamp first. 
    //lastRecordFromPreviousKeySetPage = secondKeySetPaginatedPage[^1];
    //var thirdKeySetPaginatedPage = allLogs.KeysetPagination(item => item.TimeStamp, lastRecordFromPreviousKeySetPage.TimeStamp, lastRecordFromPreviousKeySetPage.Id, pageSize: pageSize);
    //using (Assert.EnterMultipleScope())
    //{
    //    Assert.That(thirdKeySetPaginatedPage, Has.Count.EqualTo(pageSize));
    //    Assert.That(thirdKeySetPaginatedPage.ConvertAll(s => s.Id), Is.EqualTo(secondNewest3Logs.ConvertAll(s => s.Id)).AsCollection);
    //}


    //var lastDateTime = DateTime.Now.AddDays(-15);
    //var lastId = 2;
    //var anotherthing = GetLogs().KeysetPagination<Log, DateTime>(item => item.TimeStamp, lastDateTime, 2, 10);
    ////var thing = GetKeysetPaginationTest1<Log, DateTime>(item => item.TimeStamp, lastDateTime, 2, 10);
    //var a = 1;

}