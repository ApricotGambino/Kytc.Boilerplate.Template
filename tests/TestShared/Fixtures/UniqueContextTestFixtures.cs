namespace TestShared.Fixtures;
/// <summary>
/// This nUnit test fixture is used for tests that create a new context for each test.
/// That means these tests are going to be slow, because they are going to create and delete
/// the database for each test.  Sorry about that, but that's what we're testing. 
/// </summary>
[Category(TestingCategoryConstants.UniqueContextTests)]
public abstract class UniqueContextTestFixture : BaseTestFixture
{
    public override Task TestSetUpAsync()
    {
        //We want to make sure that the context is fresh prior to each test here. 
        return TestingContext.TearDownTestContextAsync();
    }

    public override async Task RunBeforeAnyTestsAsync()
    {
        //Intentionally left blank, feel free to add whatever you like, this is ran after every test.
    }

    public override Task RunAfterAnyTestsAsync()
    {
        return TestingContext.TearDownTestContextAsync();
    }
}
