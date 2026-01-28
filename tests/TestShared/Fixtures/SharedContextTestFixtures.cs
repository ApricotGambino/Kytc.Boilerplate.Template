namespace TestShared.Fixtures;
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
}

/// <summary>
/// This nUnit test fixture is used for tests that create a new context for each test.
/// That means these tests are going to be slow, because they are going to create and delete
/// the database for each test.  Sorry about that, but that's what we're testing. 
/// Also, this fixture is used for performance tests, so these tests really could be slow. 
/// </summary>
[Category(TestingCategoryConstants.PerformanceTests)]
public abstract class SharedContextPerformanceTestFixture : BaseTestFixture
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