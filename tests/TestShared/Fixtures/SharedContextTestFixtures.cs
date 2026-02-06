namespace TestShared.Fixtures;

using System.Threading.Tasks;
using Domain.Entities.Common;
using NUnit.Framework;

/// <summary>
/// This nUnit test fixture is used for tests that create a new context for each test.
/// That means these tests are going to be slow, because they are going to create and delete
/// the database for each test.  Sorry about that, but that's what we're testing. 
/// </summary>
public abstract class SharedContextTestFixture : BaseTestFixture
{
    public override async Task TestSetUpAsync()
    {
        //Intentionally left blank, feel free to add whatever you like, this is ran after every test.
    }

    public override Task RunBeforeAnyTestsAsync()
    {
        return TestingContext.SetupTestContextAsync();
    }

    public override Task RunAfterAnyTestsAsync()
    {
        return TestingContext.TearDownTestContextAsync();
    }
    //protected static async Task<Task<int>> SeedRangeAsync<TEntity>(TEntity[] entities) where TEntity : class
    //{
    //    var dbContext = TestingContext.GetService<ApplicationDbContext>();
    //    await dbContext.AddRangeAsync(entities);
    //    return dbContext.SaveChangesAsync();
    //}
    protected static async Task<int> SeedRangeAsync<TEntity>(List<TEntity> entitiesToAdd) where TEntity : BaseEntity
    {
        var dbContext = TestingContext.GetService<TestingDatabaseContext>();
        await dbContext.AddRangeAsync(entitiesToAdd);
        var addedEntitiesCount = await dbContext.SaveChangesAsync();
        Assume.That(entitiesToAdd, Has.Count.EqualTo(addedEntitiesCount), $"Failed to seed all {entitiesToAdd.Count} {typeof(TEntity)} entities, test result cannot be asserted.");

        return addedEntitiesCount;
    }

}

/// <summary>
/// This nUnit test fixture is used for tests that create a new context for each test.
/// That means these tests are going to be slow, because they are going to create and delete
/// the database for each test.  Sorry about that, but that's what we're testing. 
/// Also, this fixture is used for performance tests, so these tests really could be slow. 
/// </summary>
[Category(TestingCategoryConstants.PerformanceTests)]
public abstract class SharedContextPerformanceTestFixture : SharedContextTestFixture
{
    public override async Task TestSetUpAsync()
    {
        //Intentionally left blank, feel free to add whatever you like, this is ran after every test.
    }

    public override Task RunBeforeAnyTestsAsync()
    {
        return TestingContext.SetupTestContextAsync(TestingConstants.PerformanceUnitTestEnvironmentName);
    }

    public override Task RunAfterAnyTestsAsync()
    {
        return TestingContext.TearDownTestContextAsync();
    }
}
