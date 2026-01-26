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
