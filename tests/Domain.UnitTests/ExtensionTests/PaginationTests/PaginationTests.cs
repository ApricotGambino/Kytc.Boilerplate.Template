//namespace Domain.UnitTests.ExtensionTests.PaginationTests;

//using System;


////NOTE:
////These tests compare the results of the offset and keyset pagination strategies,
////and we expect the results of one strategy to match the other, but that's because
////we have organized the tests to make that work.
////You SHOULD NOT assume ordering by offset and keyset will yield the same results, as
////they probably won't. There's a futher description of how these two work together in the
////notes on the PaginationExtension.cs file.

//public class PaginationTests : PaginationTestFixture
//{
//    [Test]
//    public async Task PaginationComparisons_GetFirstPageOfDataOrderedByOldestFirst_BothOffsetAndKeysetPagesMatch()
//    {
//        //Arrange & Act
//        var keysetPaginatedOldestFirstPageOfRecords = _paginationTestRecords.GetNextPageUsingKeysetPagination(property => property.CreatedDateTimeOffset, DateTime.MinValue, int.MinValue, pageSize: _pageSize);
//        var offsetPaginatedOldestFirstPageOfRecords = _paginationTestRecords.OffsetPagination(property => property.CreatedDateTimeOffset, pageNumber: 1, pageSize: _pageSize);

//        //Assert
//        Assert.That(keysetPaginatedOldestFirstPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(offsetPaginatedOldestFirstPageOfRecords.ConvertAll(s => s.Id)).AsCollection);

//    }
//    [Test]
//    public async Task PaginationComparisons_GetSecondPageOfDataOrderedByOldestFirst_BothOffsetAndKeysetPagesMatch()
//    {
//        //Arrange
//        var expectedOldestFirstPageOfRecords = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).Take(_pageSize).ToList();
//        var expectedLastRecordFromOldestFirstPageOfRecords = expectedOldestFirstPageOfRecords[^1];

//        //Act
//        var keysetPaginatedOldestSecondPageOfRecords = _paginationTestRecords.GetNextPageUsingKeysetPagination(property => property.CreatedDateTimeOffset, expectedLastRecordFromOldestFirstPageOfRecords.CreatedDateTimeOffset, expectedLastRecordFromOldestFirstPageOfRecords.Id, pageSize: _pageSize);
//        var offsetPaginatedOldestSecondPageOfRecords = _paginationTestRecords.OffsetPagination(property => property.CreatedDateTimeOffset, pageNumber: 2, pageSize: _pageSize);

//        //Assert
//        Assert.That(offsetPaginatedOldestSecondPageOfRecords.ConvertAll(s => s.Id), Is.EqualTo(keysetPaginatedOldestSecondPageOfRecords.ConvertAll(s => s.Id)).AsCollection);

//    }
//    [Test]
//    public async Task PaginationComparisons_GetLastPageOfDataOrderedByOldestFirst_BothOffsetAndKeysetPagesMatch()
//    {
//        //Arrange
//        var expectedOldestRecordBeforeLastPage = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).TakeLast(_pageSize).First();
//        var orderedList = _paginationTestRecords.OrderBy(o => o.CreatedDateTimeOffset).ToList();
//        var pageNumber = (int)Math.Round((double)orderedList.Count / _pageSize);

//        //Act
//        var keysetPaginatedOldestLastPageOfRecords = _paginationTestRecords.GetNextPageUsingKeysetPagination(property => property.CreatedDateTimeOffset, expectedOldestRecordBeforeLastPage.CreatedDateTimeOffset, expectedOldestRecordBeforeLastPage.Id, pageSize: _pageSize);
//        var offsetPaginatedOldestLastPageOfRecords = _paginationTestRecords.OffsetPagination(property => property.CreatedDateTimeOffset, pageNumber: pageNumber, pageSize: _pageSize);

//        //Assert

//        Assert.That(keysetPaginatedOldestLastPageOfRecords, Has.Count.EqualTo(offsetPaginatedOldestLastPageOfRecords.Count));
//    }


//    [TestCase(1, new int[] { 101, 102, 103 })]
//    [TestCase(2, new int[] { 104, 105, 106 })]
//    [TestCase(3, new int[] { 107, 108, 109 })]
//    [TestCase(10, new int[] { 128, 129, 130 })]
//    [TestCase(11, new int[] { 131, 132, 133 })]
//    [TestCase(100, new int[] { })]
//    public async Task OffsetAndKeysetPagination_GetPageOfDataOrderedByANumberWithOffsetThenGetNextPagesWithKeysetPagination_ResultsMatchKnownOrder(int numberOfPages, int[] expectedFirstThreeValues)
//    {
//        //Arrange & Act
//        var pageOfData = _paginationTestRecords.OffsetPagination(o => o.ANumber, pageNumber: 1, pageSize: _pageSize);

//        for (var i = 1; i < numberOfPages; i++)
//        {
//            if (pageOfData.Count != 0)
//            {
//                pageOfData = _paginationTestRecords.GetNextPageUsingKeysetPagination(o => o.ANumber, lastValue: pageOfData.Last().ANumber, lastId: pageOfData.Last().Id, _pageSize);
//            }
//        }


//        //Assert
//        using (Assert.EnterMultipleScope())
//        {
//            Assert.That(pageOfData, Has.Count.EqualTo(expectedFirstThreeValues.Length));

//            for (var i = 0; i < expectedFirstThreeValues.Length; i++)
//            {
//                //Here we're checking that the first N records in what we found as the 'expected' results
//                //are what we specifically said they should be.  
//                //var record = expectedOldestFirstPageOfRecords.Skip(i).Take(1).First();
//                var record = pageOfData.Skip(i).Take(1).First();
//                Assert.That(record.ANumber, Is.EqualTo(expectedFirstThreeValues[i]));

//            }
//        }
//    }
//}
