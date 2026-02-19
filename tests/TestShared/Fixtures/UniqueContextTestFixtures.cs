namespace TestShared.Fixtures;
/// <summary>
/// This nUnit test fixture is used for tests that use the database, but do not want to share the database context with other tests,
/// This fixture will reset the database at the start of all tests ran.
/// </summary>
[Category(TestingCategoryConstants.UniqueContextTests)]
public abstract class UniqueContextTestFixture : BaseTestFixture
{
    /// <summary>
    /// <inheritdoc />
    /// This will reset the context using <see cref="ResetTestContextAsync"/>
    /// </summary>
    /// <returns></returns>
    public override Task TestSetUpAsync()
    {
        //We want to make sure that the context is fresh prior to each test here.
        return TestingContext.ResetTestContextAsync(TestingConstants.TestingEnvironmentName);
    }

    /// <summary>
    /// <inheritdoc />
    /// This will tear down the context using <see cref="TearDownTestContextAsync"/>
    /// </summary>
    /// <returns></returns>
    public override Task RunAfterAnyTestsAsync()
    {
        return TestingContext.TearDownTestContextAsync();
    }
}
