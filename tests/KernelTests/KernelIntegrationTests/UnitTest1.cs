namespace KernelIntegrationTests;

using KernelInfrastructure.Repositories;
using TestShared.Fixtures;
using TestShared.TestObjects;

public class Tests : SharedContextTestFixture
{

    [Test]
    public async Task Test1()
    {
        await TestingContext.InsertTestEntitiesIntoDatabaseAsync(10);
        var dbContext = TestingContext.GetService<TestingDatabaseContext>();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var testExampleService = TestingContext.GetService<ITestExampleService>();

        //var a = await testExampleService.GetMostRecentEntitiesUsingContextAsync();
        //var b = await testExampleService.GetMostRecentEntitiesUsingReadOnlyRepoAsync();

        Assert.Pass();
    }
}
