namespace TestShared.Fixtures;
/// <summary>
/// This nUnit test fixture is used for tests that create a new context for each test.
/// That means these tests are going to be slow, because they are going to create and delete
/// the database for each test.  Sorry about that, but that's what we're testing. 
/// </summary>
public class SharedContextTestFixture : BaseTestFixture
{
    public override async Task TestSetUp()
    {
        //Intentionally left blank, feel free to add whatever you like, this is ran after every test.
    }

    public override async Task RunBeforeAnyTests()
    {
        await TestingContext.SetupTestContext();
    }

    public override async Task RunAfterAnyTests()
    {
        await TestingContext.TearDownTestContext();
    }
}
