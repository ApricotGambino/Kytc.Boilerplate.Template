namespace TestShared.Fixtures;

using System.Threading.Tasks;
using Kernel.Data.Entities;
using NUnit.Framework;

/// <summary>
/// This nUnit test fixture is used for tests that use the database, but will share the database for all
/// tests. This fixture will create the database at the start of any tests ran, but will persist the database
/// throughout all tests hence the shared name. 
/// </summary>
public abstract class SharedContextTestFixture : BaseTestFixture
{
    public override Task RunBeforeAnyTestsAsync()
    {
        return TestingContext.SetupTestContextAsync(TestingConstants.TestingEnvironmentName);
    }

    public override Task RunAfterAnyTestsAsync()
    {
        return TestingContext.TearDownTestContextAsync();
    }

    /// <summary>
    /// This method is used to seed data for tests.  If that seed action fails, we will consider resulting tests inconclusive.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entitiesToAdd"></param>
    /// <returns></returns>
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
/// <inheritdoc />
/// asdf
/// </summary>
[Category(TestingCategoryConstants.BenchmarkTests)]
public abstract class BenchmarkTestFixture : SharedContextTestFixture
{
    public override Task RunBeforeAnyTestsAsync()
    {
        return TestingContext.SetupTestContextAsync(TestingConstants.BenchmarkTestsEnvironmentName);
    }
}
