namespace TestShared.Fixtures;
/// <summary>
/// This nUnit test fixture is used for tests that use the database, and will share the database for all
/// tests. This fixture will create the database at the start of any tests ran, but will persist the database
/// throughout all tests hence the shared name.
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
public abstract class SharedContextTestFixture : BaseTestFixture
{
    private static bool FirstInitializationFlag = true;

    /// <summary>
    /// <inheritdoc />
    /// This will setup the context using <see cref="SetupTestContextAsync"/>
    /// </summary>
    /// <returns></returns>
    public override Task RunBeforeAnyTestsAsync()
    {
        if (FirstInitializationFlag)
        {
            FirstInitializationFlag = false;

            return TestingContext.SetupTestContextAsync(TestingConstants.TestingEnvironmentName);
        }
        return base.RunBeforeAnyTestsAsync();
    }
}

