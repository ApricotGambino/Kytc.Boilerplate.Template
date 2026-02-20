using KernelData.Extensions.Pagination;
using TestShared.Fixtures;

namespace KernelData.Tests.ExtensionTests
{
    public class PaginationExtensionTests : BaseTestFixture
    {

        private IQueryable<int> Queryable { get; set; }


        [OneTimeSetUp]
        public async Task SeedTestData()
        {
            var intList = new List<int>();
            for (var i = 1; i < 1000; i++)
            {
                intList.Add(i);
            }
            Queryable = intList.AsQueryable();
        }

        [Test]
        [TestCaseSource(typeof(PaginationTestCases), nameof(PaginationTestCases.CustomPageSizeCases))]
        public async Task ToPaginatedResultsAsync_Get_First_Page(int customPageSize)
        {
            //Arrange
            var totalPages = (int)Math.Ceiling(Queryable.Count() / (double)customPageSize);
            var expectedItems = new List<int>();

            expectedItems = Queryable.OrderBy(o => o).Take(customPageSize).ToList();

            //Act
            var firstPage = await Queryable.OrderBy(o => o).ToPaginatedResultsAsync(1, customPageSize);

            //Act
            using (Assert.EnterMultipleScope())
            {
                Assert.That(firstPage.TotalPages, Is.EqualTo(totalPages));
                Assert.That(firstPage.TotalItems, Is.EqualTo(Queryable.Count()));
                Assert.That(firstPage.PageSize, Is.EqualTo(customPageSize));
                Assert.That(firstPage.Page, Is.EqualTo(1));
                Assert.That(firstPage.Results, Has.Count.EqualTo(customPageSize));
                Assert.That(firstPage.Results, Is.Ordered.Ascending);
                Assert.That(firstPage.Results, Is.EqualTo(expectedItems));
            }
        }

        [Test]
        [TestCaseSource(typeof(PaginationTestCases), nameof(PaginationTestCases.CustomPageSizeCases))]
        public async Task ToPaginatedResultsAsync_Get_All_Pages(int customPageSize)
        {
            //Arrange
            var totalPages = (int)Math.Ceiling(Queryable.Count() / (double)customPageSize);
            var expectedItems = new List<int>();
            expectedItems = new List<int>();

            for (var pageNumber = 1; pageNumber <= totalPages; pageNumber++)
            {
                expectedItems = Queryable.OrderBy(o => o).Skip((pageNumber - 1) * customPageSize).Take(customPageSize).ToList();

                //Act
                var page = await Queryable.OrderBy(o => o).ToPaginatedResultsAsync(pageNumber, customPageSize);

                //Act
                using (Assert.EnterMultipleScope())
                {
                    Assert.That(page.TotalPages, Is.EqualTo(totalPages));
                    Assert.That(page.TotalItems, Is.EqualTo(Queryable.Count()));
                    Assert.That(page.PageSize, Is.EqualTo(customPageSize));
                    Assert.That(page.Page, Is.EqualTo(pageNumber));
                    Assert.That(page.Results, Has.Count.EqualTo(expectedItems.Count()));
                    Assert.That(page.Results, Is.Ordered.Ascending);
                    Assert.That(page.Results, Is.EqualTo(expectedItems));
                }
            }
        }

        [Test]
        public async Task ToPaginatedResultsAsync_ValidateInputs()
        {
            var query = new List<int>().AsQueryable();

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => query.ToPaginatedResultsAsync(pageNumber: 0, pageSize: 1));
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => query.ToPaginatedResultsAsync(pageNumber: 1, pageSize: 0));
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => query.ToPaginatedResultsAsync(pageNumber: 0, pageSize: 0));

            query = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => query.ToPaginatedResultsAsync(1, 1));
        }
    }


    /// <summary>
    /// This class contains Parameterized test cases to be used in the pagination tests.
    /// </summary>
    public static class PaginationTestCases
    {
        /// <summary>
        /// These cases will provide custom page sizes, to test that no matter what page size we use, we get the results we expect.
        /// </summary>
        public static readonly object[] CustomPageSizeCases =
        [
            1, 3, 10, 25, 100
        ];
    }

}
