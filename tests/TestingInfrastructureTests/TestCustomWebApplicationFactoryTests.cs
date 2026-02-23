
using TestShared;
using TestShared.Fixtures;

namespace TestingInfrastructureTests;
/// <summary>
/// These tests ensure the functionality for <see cref="TestCustomWebApplicationFactory"/>
/// </summary>
[Category(TestingCategoryConstants.NUnitFrameworkTests)]
public class TestCustomWebApplicationFactoryTests : UniqueContextTestFixture
{
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
        var factory = new TestCustomWebApplicationFactory(TestingConstants.TestingEnvironmentName);
        //Assert
        Assert.That(factory.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }
}
