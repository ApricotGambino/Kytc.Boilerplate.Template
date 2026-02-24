using Kernel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using TestShared.Fixtures;
using TestShared.TestObjects;

namespace Kernel.Integration.Tests.RepositoryTests;

public class ReadOnlyEntityRepoTests : SharedContextTestFixture
{
    private List<TestEntity> _createdEntities { get; set; }
    private readonly int _pageSize = 3;

    [OneTimeSetUp]
    public async Task SeedTestData()
    {
        //Resetting the context just in case any other tests influence these results.
        await TestingContext.ResetTestContextAsync();
        _createdEntities = await TestingContext.InsertTestEntitiesIntoDatabaseAsync(10);
        await SchoolEntityHelper.InsertExampleSchoolSetup();
    }

    [Test]
    public async Task GetEntityQueryable_Returns_Valid_Queryable()
    {
        //Arrange
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var dbContext = TestingContext.GetTestingDatabaseContext();


        //Act
        var repoResults = await readonlyRepo.GetEntityQueryable().OrderBy(i => i.Id).Take(5).ToListAsync();
        var dbResults = await dbContext.TestEntities.OrderBy(i => i.Id).Take(5).ToListAsync();

        //Assert
        Assert.That(repoResults.Select(s => s.Id), Is.SubsetOf(dbResults.Select(s => s.Id)));
    }

    [Test]
    public async Task GetPaginatedEntityOrderedByIdOldestFirstAsync_FirstPage_Returns_ExpectedResults()
    {
        //Arrange
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();

        var recordIdsFromTheDatabaseOrderedById = await dbContext.TestEntities.OrderBy(o => o.Id).Take(_pageSize).Select(s => s.Id).ToListAsync();
        var recordIdsFromTheDatabaseOrderedByCreatedDate = await dbContext.TestEntities.OrderBy(o => o.CreatedDateTimeOffset).ThenBy(o => o.Id).Take(_pageSize).Select(s => s.Id).ToListAsync();

        //Act
        var fetchedData = await readonlyRepo.GetPaginatedEntityOrderedByIdOldestFirstAsync(1, _pageSize);
        var fetchedDataIds = fetchedData.Results!.Select(s => s.Id);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataIds, Is.Ordered.Ascending);
            Assert.That(recordIdsFromTheDatabaseOrderedByCreatedDate, Is.Ordered.Ascending);
            Assert.That(recordIdsFromTheDatabaseOrderedById, Is.EqualTo(recordIdsFromTheDatabaseOrderedByCreatedDate));
            Assert.That(fetchedDataIds, Is.EqualTo(recordIdsFromTheDatabaseOrderedById));
        }
    }

    [Test]
    public async Task GetPaginatedEntityOrderedByIdNewestFirstAsync_FirstPage_Returns_ExpectedResults()
    {
        //Arrange
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();

        var recordIdsFromTheDatabaseOrderedById = await dbContext.TestEntities.OrderByDescending(o => o.Id).Take(_pageSize).Select(s => s.Id).ToListAsync();
        var recordIdsFromTheDatabaseOrderedByCreatedDate = await dbContext.TestEntities.OrderByDescending(o => o.CreatedDateTimeOffset).ThenByDescending(o => o.Id).Take(_pageSize).Select(s => s.Id).ToListAsync();

        //Act
        var fetchedData = await readonlyRepo.GetPaginatedEntityOrderedByIdNewestFirstAsync(1, _pageSize);
        var fetchedDataIds = fetchedData.Results!.Select(s => s.Id);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataIds, Is.Ordered.Descending);
            Assert.That(recordIdsFromTheDatabaseOrderedByCreatedDate, Is.Ordered.Descending);
            Assert.That(recordIdsFromTheDatabaseOrderedById, Is.EqualTo(recordIdsFromTheDatabaseOrderedByCreatedDate));
            Assert.That(fetchedDataIds, Is.EqualTo(recordIdsFromTheDatabaseOrderedById));
        }
    }

    [Test]
    public async Task GetPaginatedEntityAscendingAsync_OrderedByAString_FirstPage_Returns_ExpectedResults()
    {
        //Arrange
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var recordIdsFromTheDatabaseOrderedByAString = await dbContext.TestEntities.OrderBy(o => o.AString).Take(_pageSize).Select(s => s.AString).ToListAsync();

        //Act
        var fetchedData = await readonlyRepo.GetPaginatedEntityAscendingAsync(1, _pageSize, o => o.AString);
        var fetchedDataStrings = fetchedData.Results.Select(s => s.AString);


        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataStrings, Is.Ordered.Ascending);
            Assert.That(recordIdsFromTheDatabaseOrderedByAString, Is.Ordered.Ascending);
            Assert.That(fetchedDataStrings, Is.EqualTo(recordIdsFromTheDatabaseOrderedByAString));
        }
    }

    [Test]
    public async Task GetPaginatedEntityDescendingAsync_OrderedByAString_FirstPage_Returns_ExpectedResults()
    {
        //Arrange
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var recordIdsFromTheDatabaseOrderedByAString = await dbContext.TestEntities.OrderByDescending(o => o.AString).Take(_pageSize).Select(s => s.AString).ToListAsync();

        //Act
        var fetchedData = await readonlyRepo.GetPaginatedEntityDescendingAsync(1, _pageSize, o => o.AString);
        var fetchedDataStrings = fetchedData.Results.Select(s => s.AString);


        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataStrings, Is.Ordered.Descending);
            Assert.That(recordIdsFromTheDatabaseOrderedByAString, Is.Ordered.Descending);
            Assert.That(fetchedDataStrings, Is.EqualTo(recordIdsFromTheDatabaseOrderedByAString));
        }
    }

    [Test]
    public async Task GetPaginatedEntityAscendingAsync_FilteredByABool_AllFalse_FirstPage_Returns_ExpectedResults()
    {
        //Arrange
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();

        //Act
        var fetchedData = await readonlyRepo.GetPaginatedEntityAscendingAsync(1, _pageSize, o => o.Id, p => !p.ABool);
        var fetchedDataBools = fetchedData.Results.Select(s => s.ABool);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataBools.Distinct().First(), Is.False); //Ensure all the records we fetched are false, as we would expect.
        }
    }

    [Test]
    public async Task GetPaginatedEntityAscendingAsync_Filter_ByABool_OrderingByAString_Returns_ExpectedResults()
    {
        //Arrange
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var recordIdsFromTheDatabaseOrderedByAString = await dbContext.TestEntities.Where(p => !p.ABool).OrderBy(o => o.AString).Take(_pageSize).Select(s => s.AString).ToListAsync();

        //Act
        var fetchedData = await readonlyRepo.GetPaginatedEntityAscendingAsync(1, _pageSize, o => o.AString, p => !p.ABool);
        var fetchedDataStrings = fetchedData.Results.Select(s => s.AString);
        var fetchedDataBools = fetchedData.Results.Select(s => s.ABool);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataBools.Distinct().First(), Is.False); //Ensure all the records we fetched are false, as we would expect.
            Assert.That(fetchedDataStrings, Is.Ordered.Ascending);
            Assert.That(recordIdsFromTheDatabaseOrderedByAString, Is.Ordered.Ascending);
            Assert.That(fetchedDataStrings, Is.EqualTo(recordIdsFromTheDatabaseOrderedByAString));
        }
    }

    [Test]
    public async Task GetSingleEntityByIdAsync_FirstRecord_Returns_ExpectedResult()
    {
        //Arrange
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();

        var firstTestEntity = await dbContext.TestEntities.FirstAsync();


        //Act
        var singleById = await readonlyRepo.GetSingleEntityByIdAsync(firstTestEntity.Id);

        //Assert
        Assert.That(singleById.Id, Is.EqualTo(firstTestEntity.Id));
    }

    [Test]
    public async Task GetSingleEntityByIdAsync_NonExistentRecord_Throws_Error()
    {
        //Arrange
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        //Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => readonlyRepo.GetSingleEntityByIdAsync(-1));
    }

    [Test]
    public async Task GetFirstOrDefaultEntityByFilterAsync_FirstRecord_Returns_ExpectedResult()
    {
        //Arrange
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();

        var firstTestEntity = await dbContext.TestEntities.FirstAsync();

        var firstOrDefaultResult = await readonlyRepo.GetFirstOrDefaultEntityByFilterAsync(p => p.AStringWithNumbers == firstTestEntity.AStringWithNumbers);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(firstOrDefaultResult, Is.Not.Null);
            Assert.That(firstOrDefaultResult!.Id, Is.EqualTo(firstTestEntity.Id));
        }
    }

    [Test]
    public async Task GetFirstOrDefaultEntityByFilterAsync_NonExistentRecord_Returns_Null()
    {
        //Arrange
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();

        //Act
        var firstOrDefaultResultThatDoesntExist = await readonlyRepo.GetFirstOrDefaultEntityByFilterAsync(p => p.Id == -1);

        //Assert
        Assert.That(firstOrDefaultResultThatDoesntExist, Is.Null);
    }

    [Test]
    public async Task Get_Student_Courses_Paginated_Results_Without_Using_ToPaginatedResultsAsync()
    {
        var pageSize = 2;
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var studentReadonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<Student, TestingDatabaseContext>>();


        var firstPageOfStudentsWithCourses = await studentReadonlyRepo.GetPaginatedEntityAscendingAsync(1, pageSize, order: o => o.Id, where: null, includeEntityName: nameof(Student.Courses));

        var firstStudentOfFirstPage = firstPageOfStudentsWithCourses.Results.First();
        var firstStudentOfFirstPageCourses = await dbContext.StudentToCourses.Where(p => p.StudentId == firstStudentOfFirstPage.Id).Select(s => s.Course).ToListAsync();

        Assert.That(firstStudentOfFirstPage.Courses.Select(s => s.Id), Is.EqualTo(firstStudentOfFirstPageCourses.Select(s => s.Id)));
    }
}
