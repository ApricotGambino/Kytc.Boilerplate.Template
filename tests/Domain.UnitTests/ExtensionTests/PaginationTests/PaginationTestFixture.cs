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
        //Intentionally left blank, feel free to add whatever you like, this is ran after every test.
        _paginationTestRecords = GetPaginationTestData();
    }

    private static List<TestObjectUsingBaseEntity> GetPaginationTestData()
    {
        var now = DateTime.Now;
        var listOfObjects = new List<TestObjectUsingBaseEntity>();

        for (int i = 1; i <= _numberPaginationRecords; i++)
        {
            listOfObjects.Add(new TestObjectUsingBaseEntity
            {
                Id = i,
                AString = $"Test String {i}",
                ANumber = i + 100,
                ABool = i % 2 == 0,
                CreatedDate = now.AddDays(i - _numberPaginationRecords)
            });
        }

        return listOfObjects;
    }
}
