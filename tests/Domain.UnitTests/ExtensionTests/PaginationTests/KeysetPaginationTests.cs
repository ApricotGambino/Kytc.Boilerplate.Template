
namespace Domain.UnitTests.ExtensionTests.PaginationTests;

using System;
using System.ComponentModel;
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
        var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).Take(_pageSize).ToList();

        //Act
        var keysetPaginatedOldestFirstPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.CreatedDateTimeOffset, DateTime.MinValue, int.MinValue, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestFirstPageOfRecords, Has.Count.EqualTo(_pageSize));
            Assert.That(expectedOldestFirstPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset)));
            Assert.That(keysetPaginatedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedOldestFirstPageOfRecords.Count));
            Assert.That(expectedOldestFirstPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(keysetPaginatedOldestFirstPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }
    [Test]
    public async Task KeySetPagination_GetSecondPageOfDataOrderedByOldestFirst_ResultsMatchKnownOrder()
    {
        //Arrange
        var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).Take(_pageSize).ToList();
        var expectedOldestSecondPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).Skip(_pageSize).Take(_pageSize).ToList();
        var expectedLastRecordFromOldestFirstPageOfRecords = expectedOldestFirstPageOfRecords[^1];

        //Act
        var keysetPaginatedOldestSecondPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.CreatedDateTimeOffset, expectedLastRecordFromOldestFirstPageOfRecords.CreatedDateTimeOffset, expectedLastRecordFromOldestFirstPageOfRecords.Id, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestSecondPageOfRecords, Has.Count.EqualTo(_pageSize));
            Assert.That(expectedOldestSecondPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset)));
            Assert.That(keysetPaginatedOldestSecondPageOfRecords, Has.Count.EqualTo(expectedOldestSecondPageOfRecords.Count));
            Assert.That(expectedOldestSecondPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(keysetPaginatedOldestSecondPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }
    [Test]
    public async Task KeySetPagination_GetLastPageOfOrderedDataByOldestFirst_ResultsMatchKnownOrder()
    {
        //Arrange
        var expectedOldestLastPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).TakeLast(_pageSize).ToList();
        var expectedOldestRecordBeforeLastPage = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).TakeLast(_pageSize + 1).First();

        //Act
        var keysetPaginatedOldestLastPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.CreatedDateTimeOffset, expectedOldestRecordBeforeLastPage.CreatedDateTimeOffset, expectedOldestRecordBeforeLastPage.Id, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestLastPageOfRecords, Has.Count.EqualTo(_pageSize));
            Assert.That(expectedOldestLastPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset)));
            Assert.That(keysetPaginatedOldestLastPageOfRecords, Has.Count.EqualTo(expectedOldestLastPageOfRecords.Count));
            Assert.That(keysetPaginatedOldestLastPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(expectedOldestLastPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }
    [Test]
    public async Task KeySetPagination_PaginateForwardByOldestWithPageSizeGreaterThanRemainingData_ReturnsASmallerPage()
    {
        //Arrange
        var oldestLastPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).TakeLast(_pageSize).ToList();
        var expectedOldestRecordInLastPageOfRecords = oldestLastPageOfRecords[0];
        var expectedOldestLastPageOfRecords = oldestLastPageOfRecords.OrderBy(o => o.CreatedDateTimeOffset).Skip(1).ToList();

        //Act
        var keysetPaginatedOldestLastPageOfRecords = _paginationTestRecords.KeysetPagination(item => item.CreatedDateTimeOffset, expectedOldestRecordInLastPageOfRecords.CreatedDateTimeOffset, expectedOldestRecordInLastPageOfRecords.Id, pageSize: _pageSize);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(expectedOldestLastPageOfRecords, Has.Count.EqualTo(_pageSize - 1));
            Assert.That(expectedOldestLastPageOfRecords, Is.Ordered.By(nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset)));
            Assert.That(keysetPaginatedOldestLastPageOfRecords, Has.Count.EqualTo(expectedOldestLastPageOfRecords.Count));
            Assert.That(keysetPaginatedOldestLastPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(expectedOldestLastPageOfRecords.ConvertAll(s => s.Id)).AsCollection);
        }
    }








    [TestCaseSource(nameof(GetFirstPageOfDataOrderedByProperAscendingCases))]
    public async Task KeySetPagination_GetFirstPageOfDataOrderedByPropertyAscending_ResultsMatchKnownOrder(string orderByProperty, string lastValue, string[] expectedFirstThreeValues)
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
            lastValue = DateTimeOffset.Now.AddDays(int.Parse(lastValue)).ToString();
        }

        var orderByLastValue = TypeDescriptor.GetConverter(orderByPropertyAsType).ConvertFromString(lastValue);

        if (orderByLastValue == null)
        {
            throw new InvalidOperationException($"{nameof(orderByLastValue)} cannot be null");
        }

        //Act
        var keysetPaginatedOldestFirstPageOfRecords =
            _paginationTestRecords.KeysetPagination(orderByPropertyFunction, (IComparable)orderByLastValue, lastId: int.MinValue, pageSize: _pageSize);


        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(keysetPaginatedOldestFirstPageOfRecords, Has.Count.EqualTo(expectedFirstThreeValues.Length));

            for (var i = 0; i < expectedFirstThreeValues.Length; i++)
            {
                //Here we're checking that the first N records in what we found as the 'expected' results
                //are what we specifically said they should be.  
                //var record = expectedOldestFirstPageOfRecords.Skip(i).Take(1).First();
                var record = keysetPaginatedOldestFirstPageOfRecords.Skip(i).Take(1).First();
                var expectedParameterValue = expectedFirstThreeValues[i];
                //var orderByLastValue = TypeDescriptor.GetConverter(orderByPropertyAsType).ConvertFromString(lastValue);
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

    private static readonly object[] GetFirstPageOfDataOrderedByProperAscendingCases =
    [
        //NOTE: These tests are parameterized, and the last parameter represents what we expect the first three results as the ordered type to be.
        //Also, pulled all these cases into this TestCaseSource, because there's a lot of them, and they clutter up the Test method horribly.
        //==========ABool Cases==========
        new object [] {nameof(TestObjectUsingBaseEntity.ABool), "False", new string[] { "False", "False", "False" }},
        new object [] {nameof(TestObjectUsingBaseEntity.ABool), "True", new string[] { "True", "True", "True" }},

        new object [] {nameof(TestObjectUsingBaseEntity.ANumber), "101", new string[] { "101", "102", "103" }},
        new object [] {nameof(TestObjectUsingBaseEntity.ANumber), "102", new string[] { "102", "103", "104" }},
        new object [] {nameof(TestObjectUsingBaseEntity.ANumber), "105", new string[] { "105", "106", "107" }},
        //0 doesn't exist in the list, but it comes 'before' all the other numbers, so we expect the first page.
        new object [] {nameof(TestObjectUsingBaseEntity.ANumber), "0", new string[] { "101", "102", "103" }},
        //-1 doesn't exist in the list, but it comes 'before' all the other numbers, so we expect the first page.
        new object [] {nameof(TestObjectUsingBaseEntity.ANumber), "-1", new string[] { "101", "102", "103" }},
        //1000000 doesn't exist in the list, but it comes 'after' all the other numbers, so we should not expect any values.
        new object [] {nameof(TestObjectUsingBaseEntity.ANumber), "1000000", Array.Empty<string>()},

        //==========AString Cases==========
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "A", new string[] { "A", "AA", "B" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "AA", new string[] { "AA", "B", "BB" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "B", new string[] { "B", "BB", "C" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "BB", new string[] { "BB", "C", "CC" }},
        //"" doesn't exist in the list, but it comes 'before' all the other strings, so we should expect the first page.
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "", new string[] { "A", "AA", "B" }},
        //"" doesn't exist in the list, but it comes 'before' all the other strings, so we should expect the first page.
        new object [] {nameof(TestObjectUsingBaseEntity.AString), " ", new string[] { "A", "AA", "B" }},
        //"ZZZZZZZZ" doesn't exist in the list, but it comes 'after' all the other strings, so we should not expect any values.
         new object [] {nameof(TestObjectUsingBaseEntity.AString), "ZZZZZZZZ", Array.Empty<string>()},
        //"BeeBop" doesn't exist in the list, but let's look at this:
        //Given we have this list of strings by including 'BeeBop':
        //A, B, C, D ... , AA, BB, CC, DD, ..., BeeBop
        //If we ordered them, they'd become:
        //A, AA, B, BB, BeeBop, C, CC, D, DD, ...

        //So since this list doesn't have 'BeeBop', ordering by it would look at the 'B' values in the list,
        //Find B, and BB, then compare those two to 'BeeBop' and come to the conclusion that 'B' > 'BeeBop',
        //and that 'BB' > 'BeeBop', so naturally, we must be looking for anything after the 'B's in the list,
        //which means we want the next values, and that would be: C, CC, D, DD, E, so on and so forth, and this
        //holds for any values that don't exist, for example, 'a', doesn't exist in the list, becuase it only has 'A'
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "BeeBop", new string[] { "C", "CC", "D" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "BEEBOP", new string[] { "C", "CC", "D" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "BAEBOP", new string[] { "BB", "C", "CC" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "BaEBOP", new string[] { "BB", "C", "CC" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "a", new string[] { "A", "AA", "B" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "b", new string[] { "B", "BB", "C" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "aA", new string[] { "AA", "B", "BB" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AString), "bA", new string[] { "BB", "C", "CC" }},

        //==========AStringWithNumbers Cases==========
        //string comparison with numbers in the value is weird for the same reason as string comparison is weird, but it is alphabetical also by the number...So a string list of 9,10,11 would be sorted as: 10,11,9.
        //I hope the lesson with the AStringWithNumber sorting test, that you can see why you should NEVER use a string field to sort if it contains numbers.  Look at 'Test String 19' for a great example of how hard it is to
        //conceptualize.
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "Test String 1", new string[] { "Test String 1", "Test String 10", "Test String 11" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "Test String 10", new string[] { "Test String 10", "Test String 11", "Test String 12" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "Test String 30", new string[] { "Test String 30", "Test String 31", "Test String 32" }},
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "Test String 19", new string[] { "Test String 19", "Test String 2", "Test String 20" }},
        //"" doesn't exist in the list, but it comes 'before' all the other strings, so we should expect the first page.
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "", new string[] { "Test String 1", "Test String 10", "Test String 11" }},
        //"Test String -1" doesn't exist in the list, but it comes 'before' all the other strings, so we should expect the first page.
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "Test String -1", new string[] { "Test String 1", "Test String 10", "Test String 11" }},
        //"Test String Z" doesn't exist in the list, but it comes 'before' all the other strings, so we should not expect any values.
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "Test String Z", Array.Empty<string>()},
        //Test String 9999 doesn't exist in the list, but it comes 'after' all the other numbers, so we should not expect any values.
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "Test String 9999", Array.Empty<string>()},
        //Test String ZZZZZZZ doesn't exist in the list, but it comes 'after' all the other numbers, so we should not expect any values.
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "ZZZZZZZ", Array.Empty<string>()},
        //"Test String 1Z" doesn't exist in the list, but, because the order would be "Test String 1", "Test String 10", "Test String 11", "Test String 11", "Test String 1Z" ->  "Test String 2" 
        new object [] {nameof(TestObjectUsingBaseEntity.AStringWithNumbers), "Test String 1Z", new string[] { "Test String 2", "Test String 20", "Test String 21"}},

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
        new object [] {nameof(TestObjectUsingBaseEntity.ADateTimeOffset), "0", new string[] { "50" }},
        new object [] {nameof(TestObjectUsingBaseEntity.ADateTimeOffset), "-1", new string[] { "49", "50" }},
        new object [] {nameof(TestObjectUsingBaseEntity.ADateTimeOffset), "-2", new string[] { "48", "49", "50" }},
        new object [] {nameof(TestObjectUsingBaseEntity.ADateTimeOffset), "-3", new string[] { "47", "48", "49" }},
        //This record doesn't exist in the list, and it comes 'after' all the other dates, so we should not expect any values.
        new object [] {nameof(TestObjectUsingBaseEntity.ADateTimeOffset), "1", Array.Empty<string>()},
        //This record doesn't exist in the list, and it comes 'before' all the other dates, so we should expect the first page.
        new object [] {nameof(TestObjectUsingBaseEntity.ADateTimeOffset), "-100", new string[] { "1", "2", "3" }},


        //==========CreatedDateTimeOffset Cases==========
        //Testing CreatedDate shouldn't be any different from ADateTimeOffset,
        //but the CreatedDate was simulated 'in a batch' all at once, so the values don't increase over time
        //like our contrived ADateTimeOffset example    
        new object [] {nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset), "0", new string[] { "1", "2", "3" }},
        //This record doesn't exist in the list, and it comes 'before' all the other dates, so we should expect the first page.
        new object [] {nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset), "-1", new string[] { "1", "2", "3" }},
        //This record doesn't exist in the list, and it comes 'after' all the other dates, so we should not expect any values.
        new object [] {nameof(TestObjectUsingBaseEntity.CreatedDateTimeOffset), "1", Array.Empty<string>()},
    ];
}
