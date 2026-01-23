namespace TestShared.Tests;

using TestShared;
using TestShared.Fixtures;

[Category(TestingCategoryConstants.nUnitFrameworkTests)]
public class TestCustomWebApplicationFactoryTests : UniqueContextTestFixture
{
    //NOTE: These tests ONLY test the functionality that the TestCustomWebApplicationFactory provides.
    //This distinction exists because this webapplicationfactory uses Program.cs, and we'll test elsewhere.
    [Test]
    public void TestCustomWebApplicationFactoryConstructor_EmptyEnvironmentName_ThrowsError()
    {
        //Assert & Act
        Assert.Throws<ArgumentNullException>(
            () => new TestCustomWebApplicationFactory(environmentName: string.Empty));
    }

    [Test]
    public void TestCustomWebApplicationFactoryConstructor_DefaultEnvironmentName_FactoryHasEnvironmentName()
    {
        //Act
        var factory = new TestCustomWebApplicationFactory(TestingConstants.EnvironmentName);
        //Assert
        Assert.That(factory.EnvironmentName, Is.EqualTo(TestingConstants.EnvironmentName));
    }
}
