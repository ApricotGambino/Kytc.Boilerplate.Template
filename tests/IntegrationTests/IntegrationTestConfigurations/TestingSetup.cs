namespace IntegrationTests.IntegrationTestConfigurations;


using NUnit.Framework;
using NUnit.Framework.Internal;

/// <summary>
/// This class' scope is applied to all tests within the namespace within the assembly.
/// https://docs.nunit.org/articles/nunit/writing-tests/attributes/setupfixture.html
/// </summary>
[SetUpFixture]
public class TestingSetup
{

    //NOTE:
    //The scope of a SetUpFixture is limited to an assembly.
    //A SetUpFixture in a namespace will apply to all tests in that namespace and all contained namespaces within the assembly.
    //A SetUpFixture outside of any namespace provides SetUp and TearDown for the entire assembly.
    //When a test is ran, nUnit (very generally) works like this:
    // Find [SetUpFixture] (This class), run that class.
    // Inside of the [SetUpFixture] class, run the [OneTimeSetUp] decorated method.
    // For each test that's ran, run the [TestFixture] (BaseTestFixture.cs) class, and the [SetUp] method inside that.
    // After all tests have ran, run the [SetUpFixture] class's [OneTimeTearDown] decorated method.


    /// <summary>
    /// This will be ran once before any tests are ran.  This is not ran for every test. 
    /// </summary>
    /// <returns></returns>
    [OneTimeSetUp]
    public async Task RunBeforeAnyTests() => await TestContext.SetupTestContext();

    /// <summary>
    /// This will be ran once after any tests are ran.  This is not ran for every test. 
    /// </summary>
    /// <returns></returns>
    [OneTimeTearDown]
    public async Task RunAfterAnyTests() => await TestContext.TearDownTestContext();
}
