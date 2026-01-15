#pragma warning disable IDE0130 // Namespace does not match folder structure, supressing because this is intentional. 
namespace Kytc.Boilerplate.Template.IntegrationTests;
#pragma warning restore IDE0130 // Namespace does not match folder structure, supressing because this is intentional. 

using NUnit.Framework;

/// <summary>
/// This is the base fixture for all tests.
/// https://docs.nunit.org/articles/nunit/writing-tests/attributes/testfixture.html
/// </summary>
[TestFixture]
public class BaseTestFixture
{
    /// <summary>
    /// This method is ran before every test. 
    /// </summary>
    /// <returns></returns>
    [SetUp]
    public async Task TestSetUp()
    {
        //Intentionally left blank, feel free to add whatever you like.
        //You could set this up to completely reset the test database for every test as an example, but the more you do here, the slower your tests will be ran.     
    }
}
