namespace TestShared.Fixtures;
///// <summary>
///// This class' scope is applied to all tests within the namespace within the assembly.
///// https://docs.nunit.org/articles/nunit/writing-tests/attributes/setupfixture.html
///// </summary>
[SetUpFixture]
public abstract class BaseSetUpFixture
{
    //NOTE: This class is abstract so we can override methods, since not all tests want to be ran the same way, but most want to be ran a certain way. An example of this would
    //be the unique contexttestfixture that needs to create and delete the database every test. 

    //NOTE: If you're curious how these nUnit fixtures are 'called':

    //According to the documentation: https://docs.nunit.org/articles/nunit/writing-tests/attributes/setupfixture.html
    //The defined order is as follows...
    //1) Setup starts at the assembly level SetUpFixture, outside of any namespace.
    //2) It continues with the top level of any SetUpFixtures in a namespace , proceeds downward into any nested namespaces.
    //3) Setup code in a TestFixture comes after any SetUpFixtures that control the namespace of the fixture.
    //4) At each of the above levels, inheritance may also come into play.Base class setups are run before those of the derived class.
    //5) Teardown for any of the above executes in the reverse order.
    //6) Ordering of TestFixtures or SetUpFixtures within the same namespace is indeterminate.
    //7) Ordering of multiple setup methods within the same class is indeterminate.
    //[Items 6 and 7 rarely come into play but the features are available for situations like code generation, where it may be more convenient to have multiple setup fixtures and / or methods.]    

    /// <summary>
    /// This will be ran once before any tests are ran.  This is not ran for every test. 
    /// </summary>
    /// <returns></returns>
    [OneTimeSetUp]
    public virtual async Task RunBeforeAnyTests()
    {
        await TestingContext.SetupTestContext();
    }

    /// <summary>
    /// This will be ran once after any tests are ran.  This is not ran for every test. 
    /// </summary>
    /// <returns></returns>
    [OneTimeTearDown]
    public virtual async Task RunAfterAnyTests()
    {
        await TestingContext.TearDownTestContext();
    }
}
