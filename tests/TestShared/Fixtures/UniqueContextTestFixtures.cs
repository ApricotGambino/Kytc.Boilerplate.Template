namespace TestShared.Fixtures;
/// <summary>
/// This nUnit test fixture is used for tests that use the database, but do not want to share context with other tests,
/// This fixture will create the database at the start of all tests ran.
/// </summary>
[Category(TestingCategoryConstants.UniqueContextTests)]
public abstract class UniqueContextTestFixture : BaseTestFixture
{
    public override Task TestSetUpAsync()
    {
        //We want to make sure that the context is fresh prior to each test here. 
        return TestingContext.ResetTestContextAsync(TestingConstants.TestingEnvironmentName);
    }

    public override Task RunAfterAnyTestsAsync()
    {
        return TestingContext.TearDownTestContextAsync();
    }
}
