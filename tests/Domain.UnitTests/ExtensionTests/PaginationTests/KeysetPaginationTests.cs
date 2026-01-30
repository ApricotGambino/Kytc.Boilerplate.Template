
namespace Domain.UnitTests.ExtensionTests.PaginationTests;

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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

    //[Test]
    //public async Task KeySetPagination_GetFirstPageOfDataOrderedById_ResultsMatchKnownOrder()
    //{
    //    //Arrange
    //    var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.Id).Take(_pageSize).ToList();

    //    //Act
    //    var keysetPaginatedOldestFirstPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.Id, int.MinValue, int.MinValue, pageSize: _pageSize);

    //    //Assert
    //    using (Assert.EnterMultipleScope())
    //    {
    //        Assert.That(expectedOldestFirstPageOfRecords, Has.Count.EqualTo(_pageSize));
    //        Assert.That(expectedOldestFirstPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDate)));
    //        Assert.That(keysetPaginatedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedOldestFirstPageOfRecords.Count));
    //        Assert.That(expectedOldestFirstPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(keysetPaginatedOldestFirstPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
    //    }
    //}
    //[Test]
    //public async Task KeySetPagination_GetFirstPageOfDataOrderedByANumber_ResultsMatchKnownOrder()
    //{
    //    //Arrange
    //    var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.ANumber).Take(_pageSize).ToList();

    //    //Act
    //    var keysetPaginatedOldestFirstPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.ANumber, int.MinValue, int.MinValue, pageSize: _pageSize);

    //    //Assert
    //    using (Assert.EnterMultipleScope())
    //    {
    //        Assert.That(expectedOldestFirstPageOfRecords, Has.Count.EqualTo(_pageSize));
    //        Assert.That(expectedOldestFirstPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDate)));
    //        Assert.That(keysetPaginatedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedOldestFirstPageOfRecords.Count));
    //        Assert.That(expectedOldestFirstPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(keysetPaginatedOldestFirstPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
    //    }
    //}
    //[Test]
    //public async Task KeySetPagination_GetFirstPageOfDataOrderedByABool_ResultsMatchKnownOrder()
    //{
    //    //Arrange
    //    var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.ABool).Take(_pageSize).ToList();

    //    //Act
    //    var keysetPaginatedOldestFirstPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.ABool, false, int.MinValue, pageSize: _pageSize);

    //    //Assert
    //    using (Assert.EnterMultipleScope())
    //    {
    //        Assert.That(expectedOldestFirstPageOfRecords, Has.Count.EqualTo(_pageSize));
    //        Assert.That(expectedOldestFirstPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDate)));
    //        Assert.That(keysetPaginatedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedOldestFirstPageOfRecords.Count));
    //        Assert.That(expectedOldestFirstPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(keysetPaginatedOldestFirstPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
    //    }
    //}

    //public static IQueryable<T> ContainsByField<T>(this IQueryable<T> q, string field, string value)
    //{
    //    var eParam = Expression.Parameter(typeof(T), "e");
    //    //var method = field.GetType().GetMethod("Contains");
    //    var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
    //    var call = Expression.Call(Expression.Property(eParam, field), method, Expression.Constant(value));
    //    var lambdaExpression = Expression.Lambda<Func<T, bool>>(
    //        Expression.AndAlso(
    //            Expression.NotEqual(Expression.Property(eParam, field), Expression.Constant(null)),
    //            call
    //        ),
    //        eParam
    //    );

    //    return q.Where(lambdaExpression);
    //}

    [Test]
    public async Task GetValueFromTestData_RecordThatDoesExistAndRecordThatDoesntExist_ReturnsFoundAndNotFound()
    {
        var foundResult = GetValueFromTestData(nameof(TestObjectUsingBaseEntity.ABool), "false");
        //var missingResult = GetValueFromTestData("Id", "-1");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(foundResult.Id, Is.EqualTo(1));
            //Assert.That(foundResult, Is.EqualTo(null));
        }
    }

    private TestObjectUsingBaseEntity GetValueFromTestData(string orderByProperty, string orderByValue)
    {
        //This method is gross, but I needed to make it so I could parameterize test cases
        //to test everything expected from the KeysetPagination method. (Because I can't use hardened values, only strings) This is not even remotely maintainable code. 
        //Also, I completely ripped this code off from: https://stackoverflow.com/questions/59666644/linq-how-to-combine-reflection-with-where-and-contains
        var eParam = Expression.Parameter(typeof(TestObjectUsingBaseEntity), "e");
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var call = Expression.Call(Expression.Property(eParam, orderByProperty), method, Expression.Constant(orderByValue));
        var lambdaExpression = Expression.Lambda<Func<TestObjectUsingBaseEntity, bool>>(
            Expression.AndAlso(
                Expression.NotEqual(Expression.Property(eParam, orderByProperty), Expression.Constant(null)),
                call
            ),
            eParam
        );

        return _paginationTestRecords.AsQueryable().Where(lambdaExpression).FirstOrDefault();
    }

    [TestCase(nameof(TestObjectUsingBaseEntity.ABool), "false")]
    [TestCase(nameof(TestObjectUsingBaseEntity.ABool), "True")]
    [TestCase(nameof(TestObjectUsingBaseEntity.ANumber), "0")]
    [TestCase(nameof(TestObjectUsingBaseEntity.ANumber), "105")]
    [TestCase(nameof(TestObjectUsingBaseEntity.AString), "")]
    [TestCase(nameof(TestObjectUsingBaseEntity.AString), "BeeBop")]
    [TestCase(nameof(TestObjectUsingBaseEntity.AString), "Test String 1")]
    [TestCase(nameof(TestObjectUsingBaseEntity.AString), "Test String 30")]
    [TestCase(nameof(TestObjectUsingBaseEntity.AString), "Test String 9")]
    [TestCase(nameof(TestObjectUsingBaseEntity.CreatedDate), "1/30/2026 6:08:09 PM -05:00")]
    public async Task KeySetPagination_GetFirstPageOfDataOrderedByProperty_ResultsMatchKnownOrder(string orderByProperty, string lastValue)
    {
        //NOTE: Developers in the future, please forgive me for I have sinned with this method.  I know it's unreadable, and messy.
        //This was all done so we could parameterize this test and throw as many types and values at it as we wanted...
        //Arrange

        Func<TestObjectUsingBaseEntity, IComparable> orderByPropertyFunc = s => (IComparable)s.GetType().GetProperty(orderByProperty.Trim()).GetValue(s);
        var orderByPropertyAsType = typeof(TestObjectUsingBaseEntity).GetProperty(orderByProperty.Trim()).PropertyType;

        //string s_test = "1/30/2026 6:08:09 PM -05:00";
        //DateTimeOffset m_test;
        //m_test = (DateTimeOffset)TypeDescriptor.GetConverter(typeof(DateTimeOffset)).ConvertFromString(s_test);


        var orderByLastValue = TypeDescriptor.GetConverter(orderByPropertyAsType).ConvertFromString(lastValue);

        //IComparable orderByLastValue = (IComparable)Convert.ChangeType(lastValue, orderByPropertyAsType);



        TestObjectUsingBaseEntity testObjectInListWithLastValue = null;
        foreach (var pageRecord in _paginationTestRecords)
        {
            var propertyValueConverted = TypeDescriptor.GetConverter(orderByPropertyAsType).ConvertFromString(pageRecord.GetType().GetProperty(orderByProperty).GetValue(pageRecord).ToString());
            //var propertyValueConvertedThenBackToString = Convert.ChangeType(pageRecord.GetType().GetProperty(orderByProperty).GetValue(pageRecord).ToString(), orderByPropertyAsType).ToString();
            //var lastValueConvertedToTypeThenToString = Convert.ChangeType(lastValue, orderByPropertyAsType).ToString();
            var lastValueConvertedToType = TypeDescriptor.GetConverter(orderByPropertyAsType).ConvertFromString(lastValue);
            if (propertyValueConverted.ToString() == lastValueConvertedToType.ToString())
            {
                testObjectInListWithLastValue = pageRecord;
                break;
            }
        }

        var allOrderedRecords = _paginationTestRecords.OrderBy(orderByPropertyFunc).ToList();

        var expectedOldestFirstPageOfRecords = allOrderedRecords.Take(_pageSize).ToList();

        if (testObjectInListWithLastValue != null)
        {
            var indexOfTestObjectInListWithLastValue = allOrderedRecords.IndexOf(testObjectInListWithLastValue);
            expectedOldestFirstPageOfRecords = allOrderedRecords.Skip(indexOfTestObjectInListWithLastValue).Take(_pageSize).ToList();
            //expectedOldestFirstPageOfRecords =  _paginationTestRecords.Where(p => p.Id >= testObjectInListWithLastValue.Id).OrderBy(orderByPropertyFunc).Take(_pageSize).ToList();
        }

        var keysetPaginatedOldestFirstPageOfRecords = _paginationTestRecords.KeysetPagination(orderByPropertyFunc, (IComparable)orderByLastValue, int.MinValue, pageSize: _pageSize);

        //Act


        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestFirstPageOfRecords, Is.Ordered.By(orderByProperty));
            Assert.That(keysetPaginatedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedOldestFirstPageOfRecords.Count));
            Assert.That(expectedOldestFirstPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(keysetPaginatedOldestFirstPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
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