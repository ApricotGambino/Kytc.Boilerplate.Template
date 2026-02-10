namespace TestShared.Fixtures;

/// <summary>
/// This is the base fixture for all tests.
/// https://docs.nunit.org/articles/nunit/writing-tests/attributes/testfixture.html
/// </summary>
[TestFixture]
public abstract class BaseTestFixture
{
    //NOTE: This class is abstract so we can override methods, since not all tests want to be ran the same way, but most want to be ran a certain way. An example of this would
    //be the unique contexttestfixture that needs to create and delete the database every test. 

    /// <summary>
    /// This method is ran before every test. 
    /// </summary>
    /// <returns></returns>
    [SetUp]
    public virtual async Task TestSetUpAsync()
    {
        //Intentionally left blank, feel free to add whatever you like. 
    }

    /// <summary>
    /// This method is ran after every test. 
    /// </summary>
    /// <returns></returns>
    [TearDown]
    public virtual async Task TestTearDownAsync()
    {
        //Intentionally left blank, feel free to add whatever you like.         
    }

    /// <summary>
    /// This method is ran before any test is ran, but only once. 
    /// </summary>
    /// <returns></returns>
    [OneTimeSetUp]
    public virtual async Task RunBeforeAnyTestsAsync()
    {
        //Intentionally left blank, feel free to add whatever you like.
    }

    /// <summary>
    /// This method is ran after any test is ran, but only once. 
    /// </summary>
    /// <returns></returns>
    [OneTimeTearDown]
    public virtual async Task RunAfterAnyTestsAsync()
    {
        //Intentionally left blank, feel free to add whatever you like.
    }
}
