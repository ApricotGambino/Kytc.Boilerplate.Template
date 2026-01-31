namespace Domain.UnitTests.ExtensionTests.PaginationTests;

using System;
using System.Collections.Generic;
using TestShared.Fixtures;
using TestShared.TestObjects;

public abstract class PaginationTestFixture : BaseTestFixture
{
    public const int _numberPaginationRecords = 50;
    public const int _pageSize = 3;
    public List<TestObjectUsingBaseEntity> _paginationTestRecords { get; private set; }

    public override async Task RunBeforeAnyTestsAsync()
    {
        _paginationTestRecords = GetPaginationTestData();
    }

    private static List<TestObjectUsingBaseEntity> GetPaginationTestData()
    {
        //NOTE: You can adjust this data generation, but because we use this data for parameterized tests,
        //specifically for testing edge cases, modifications could lead to unexpected test failures.
        //We COULD code around this, but it mean we can't use parameters, and would require a specific test case for each
        //parameter, making the test case file ENORMOUS. 
        var now = DateTime.Now;
        var listOfObjects = new List<TestObjectUsingBaseEntity>();

        for (var i = 1; i <= _numberPaginationRecords; i++)
        {
            listOfObjects.Add(new TestObjectUsingBaseEntity
            {
                Id = i,
                //The 'AString' code is stole from here: https://codereview.stackexchange.com/questions/148506/incrementing-a-sequence-of-letters-by-one
                //It just builds up a string like A->B->C...X->Y->Z->AA->BB->CC etc.
                //So that we can actually have unique values we can test order with. 
                AString = new string((char)('A' + ((i - 1) % 26)), ((i - 1) / 26) + 1),
                AStringWithNumbers = $"Test String {i}",
                ANumber = i + 100,
                ABool = i % 2 == 0,
                ADateTimeOffset = now.AddDays(i - _numberPaginationRecords)
            });
        }

        return listOfObjects;
    }
}
