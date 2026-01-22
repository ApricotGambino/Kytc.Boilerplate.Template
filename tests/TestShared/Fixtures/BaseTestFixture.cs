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
    public virtual async Task TestSetUp()
    {
        //Intentionally left blank, feel free to add whatever you like, this is ran before every test. 
    }

    /// <summary>
    /// This method is ran before every test. 
    /// </summary>
    /// <returns></returns>
    [TearDown]
    public virtual async Task TestTearDown()
    {
        //Intentionally left blank, feel free to add whatever you like, this is ran after every test.         
    }
}
