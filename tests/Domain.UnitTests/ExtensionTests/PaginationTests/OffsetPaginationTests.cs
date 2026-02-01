
namespace Domain.UnitTests.ExtensionTests.PaginationTests;

using System.ComponentModel;
using System.Linq;
using NUnit.Framework;

using TestShared.TestObjects;
using static Domain.UnitTests.ExtensionTests.PaginationTests.OffsetKeysetCommonPaginationTestCases;
public class OffsetPaginationTests : PaginationTestFixture
{
    [Test]
    public async Task OffsetPagination_GetFirstPageOfDataOrderedByOldestFirst_ResultsMatchKnownOrder()
    {
        //Arrange
        var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).Take(_pageSize).ToList();

        //Act
        var offsetPaginatedOldestFirstPageOfRecords = _paginationTestRecords.OffsetPagination(property => property.CreatedDateTimeOffset, pageNumber: 1, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestFirstPageOfRecords, Has.Count.EqualTo(_pageSize));
            Assert.That(expectedOldestFirstPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset)));
            Assert.That(offsetPaginatedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedOldestFirstPageOfRecords.Count));
            Assert.That(expectedOldestFirstPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(offsetPaginatedOldestFirstPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }
    [Test]
    public async Task OffsetPagination_GetSecondPageOfDataOrderedByOldestFirst_ResultsMatchKnownOrder()
    {
        //Arrange
        var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).Take(_pageSize).ToList();
        var expectedOldestSecondPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).Skip(_pageSize).Take(_pageSize).ToList();
        var expectedLastRecordFromOldestFirstPageOfRecords = expectedOldestFirstPageOfRecords[^1];

        //Act
        var offsetPaginatedOldestSecondPageOfRecords = _paginationTestRecords.OffsetPagination(property => property.CreatedDateTimeOffset, pageNumber: 2, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestSecondPageOfRecords, Has.Count.EqualTo(_pageSize));
            Assert.That(expectedOldestSecondPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset)));
            Assert.That(offsetPaginatedOldestSecondPageOfRecords, Has.Count.EqualTo(expectedOldestSecondPageOfRecords.Count));
            Assert.That(expectedOldestSecondPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(offsetPaginatedOldestSecondPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }
    [Test]
    public async Task OffsetPagination_GetLastPageOfOrderedDataByOldestFirst_ResultsMatchKnownOrder()
    {
        //Arrange
        var orderedList = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).ToList();
        var pageNumber = (int)Math.Round((double)orderedList.Count / _pageSize);

        var expectedOldestLastPageOfRecords = orderedList.Skip((pageNumber - 1) * _pageSize).TakeLast(_pageSize).ToList();

        //Act
        var offsetPaginatedOldestLastPageOfRecords = _paginationTestRecords.OffsetPagination(property => property.CreatedDateTimeOffset, pageNumber: pageNumber, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestLastPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset)));
            Assert.That(offsetPaginatedOldestLastPageOfRecords, Has.Count.EqualTo(expectedOldestLastPageOfRecords.Count));
            Assert.That(offsetPaginatedOldestLastPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(expectedOldestLastPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }

    [TestCaseSource(typeof(OffsetKeysetCommonPaginationTestCases), nameof(CustomPageSizeCases))]
    public async Task OffsetPagination_GetEveryPageOfDataByAStringAscendingByPageSize_ResultsMatchKnownOrder(int customPageSize)
    {
        //Arrange
        var previousExpectedRecords = new List<TestObjectUsingBaseEntity>();
        var previousoffsetPaginatedRecords = new List<TestObjectUsingBaseEntity>();
        var previousoffsetPaginatedRecord = new TestObjectUsingBaseEntity();

        //Act & Assert
        for (var i = 0; i < _paginationTestRecords.Count / customPageSize; i++)
        {
            previousExpectedRecords = _paginationTestRecords.OrderBy(o => o.AString).Skip(customPageSize * i).Take(customPageSize).ToList();

            if (previousoffsetPaginatedRecord == null)
            {
                throw new InvalidOperationException("Could not find previous paginated record.");
            }

            previousoffsetPaginatedRecords = _paginationTestRecords.OffsetPagination(property => property.AString, pageNumber: i + 1, pageSize: customPageSize);
            previousoffsetPaginatedRecord = previousoffsetPaginatedRecords.LastOrDefault();
            Assert.That(previousExpectedRecords.ConvertAll(s => s.Id), Is.EqualTo(previousoffsetPaginatedRecords.ConvertAll(s => s.Id)).AsCollection);
        }

        Assert.That(previousExpectedRecords.ConvertAll(s => s.Id), Is.EqualTo(previousoffsetPaginatedRecords.ConvertAll(s => s.Id)).AsCollection);
    }

    [TestCaseSource(nameof(GetFirstPageOfOffsetPaginatedDataOrderedByProperAscendingCases))]
    public async Task OffsetPagination_GetFirstPageOfDataOrderedByPropertyAscending_ResultsMatchKnownOrder(string orderByProperty, string[] expectedFirstThreeValues)
    {
        IComparable orderByPropertyFunction(TestObjectUsingBaseEntity s)
        {
            var propertyInfo = s.GetType().GetProperty(orderByProperty.Trim());
            if (propertyInfo != null)
            {
                var propertyValue = propertyInfo.GetValue(s);
                if (propertyValue != null)
                {
                    return (IComparable)propertyValue;
                }
            }
            throw new ArgumentException($"The {nameof(orderByProperty)} value of {orderByProperty} is not a valid property found on the {nameof(TestObjectUsingBaseEntity)} object.");
        }

        var orderByPropertyAsType = typeof(TestObjectUsingBaseEntity).GetProperty(orderByProperty.Trim())!.PropertyType;

        if (orderByPropertyAsType == typeof(DateTimeOffset))
        {
            //NOTE: For DateFields, this gets a little weird, because we can't
            //use something like 'DateTimeOffset.Now.ToString()' in the annotation, since it requires a constant,
            //and if we hard code a datetimeoffset string to something like '"1/30/2026 6:08:09 PM -05:00"', the
            //test results will be dependant on when they are ran, so instead we'll just use a value to 'AddDays' to the
            //current DatetimeOffset object. 
            //lastValue = DateTimeOffset.Now.AddDays(int.Parse(lastValue)).ToString();
        }

        //var orderByLastValue = TypeDescriptor.GetConverter(orderByPropertyAsType).ConvertFromString(lastValue);

        //if (orderByLastValue == null)
        //{
        //    throw new InvalidOperationException($"{nameof(orderByLastValue)} cannot be null");
        //}

        //Act
        var offsetPaginatedOldestFirstPageOfRecords =
            _paginationTestRecords.OffsetPagination(orderByPropertyFunction, pageNumber: 1, pageSize: _pageSize);


        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(offsetPaginatedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedFirstThreeValues.Length));

            for (var i = 0; i < expectedFirstThreeValues.Length; i++)
            {
                //Here we're checking that the first N records in what we found as the 'expected' results
                //are what we specifically said they should be.  
                //var record = expectedOldestFirstPageOfRecords.Skip(i).Take(1).First();
                var record = offsetPaginatedOldestFirstPageOfRecords.Skip(i).Take(1).First();
                var expectedParameterValue = expectedFirstThreeValues[i];
                var recordOrderByPropertyValue = record.GetType().GetProperty(orderByProperty)!.GetValue(record);

                if (recordOrderByPropertyValue != null)
                {
                    var expectedRecordValueConverted = TypeDescriptor.GetConverter(orderByPropertyAsType).ConvertFromString(recordOrderByPropertyValue.ToString() ?? string.Empty);

                    if (orderByPropertyAsType == typeof(DateTimeOffset))
                    {
                        Assert.That(record.Id.ToString(), Is.EqualTo(expectedParameterValue));
                    }
                    else
                    {
                        Assert.That(expectedRecordValueConverted?.ToString(), Is.EqualTo(expectedParameterValue));
                    }
                }
            }
        }
    }

    /// <summary>
    /// The tests are meant to be used on offset pagination tests that order by ascending given a property, last parameter represents what we expect the first three results as the ordered type to be.
    /// </summary>
    private static readonly object[] GetFirstPageOfOffsetPaginatedDataOrderedByProperAscendingCases =
    [
        //These tests are boring, and we're really just testing linq's .orderBy() function, but we do want to make
        //sure OffsetPagination never breaks expectations if it changes. 
        new object [] {nameof(TestObjectUsingBaseEntity.ABool),  new string[] { "False", "False", "False" }},
        new object [] {nameof(TestObjectUsingBaseEntity.ANumber), new string[] { "101", "102", "103" }},
        //==========AString Cases==========
        new object [] {nameof(TestObjectUsingBaseEntity.AString), new string[] { "A", "AA", "B" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers),  new string[] { "Test String 1", "Test String 10", "Test String 11" }},

        //==========DateTimeOffset Cases==========
        //For DateTimeOffset tests, it's a little tricky.
        //We use DateTimeOffset.now to create the list, and can't really pass in that value into the parameters.
        //So instead we pass in a value which will 'AddDays()' to the value we're ordering by.
        //Because if we hardcode a DateTimeOffset value as a string, these tests could fail
        //depending on when you run them. Not perfect.

        //A way to think about this list for dates is like this:
        //Id: 1, ADateTimeOffset: ThisVerySecond, but 50 days ago.
        //Id: 2, ADateTimeOffset: ThisVerySecond, but 49 days ago.
        //Id: 3, ADateTimeOffset: ThisVerySecond, but 48 days ago.
        //...
        //Id: 48, ADateTimeOffset: ThisVerySecond, but 2 days ago.
        //Id: 49, ADateTimeOffset: ThisVerySecond, but 1 days ago.
        //Id: 50, ADateTimeOffset: ThisVerySecond.

        //'ThisVerySecond' is a little misleading, because we're doing string comparisons, and nanoseconds get dropped.
        //But we're not going to test that deeply, so for now, seconds are good enough to consider. 

        //A value of '0' means just use the current DateTimeOffset value.
        //A value of '-1' means order ascending since this time yesterday.
        //Finally, the last parameter for DateTimeTests represents the ID of the record we expect. 
        new object [] {nameof(TestObjectUsingBaseEntity.ADateTimeOffset), new string[] { "1", "2", "3" }},


        //==========CreatedDateTimeOffset Cases==========
        //Testing CreatedDate shouldn't be any different from ADateTimeOffset,
        //but the CreatedDate was simulated 'in a batch' all at once, so the values don't increase over time
        //like our contrived ADateTimeOffset example    
        new object [] {nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset), new string[] { "1", "2", "3" }}
    ];
}
