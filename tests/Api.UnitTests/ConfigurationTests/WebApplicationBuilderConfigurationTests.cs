namespace Api.UnitTests.ConfigurationTests;

using Api.Configurations;
using Microsoft.AspNetCore.Builder;



[Category("ApiStartupTests")]
public class WebApplicationBuilderConfigurationTests
{
    [Test]
    public async Task AddAppSettingsJsonFile_UsingUnitTestEnvironment_FindsExpectedValue()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = "UnitTest";

        //Act
        builder.AddAppSettingsJsonFile();
        var configuration = builder.Configuration.GetSection("ValueUsedOnlyForAUnitTest").Value;

        //Act
        Assert.That(configuration, Is.EqualTo("Leave this value alone, I am just here for a unit test"));
    }

    [Test]
    public async Task AddAppSettingsJsonFile_UsingIntentionallyBadUnitTestEnvironment_DoesNotFindValueOnlyInUnitTestEnvironment()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = "IntentionallyBadUnitTest";

        //Act
        builder.AddAppSettingsJsonFile();
        var configuration = builder.Configuration.GetSection("ValueUsedOnlyForAUnitTest").Value;

        //Act
        Assert.That(configuration, Is.Null);
    }

    [Test]
    public async Task AddAppSettingsJsonFile_MissingAppSettingFile_ThrowsException()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = "ThisEnvironmentHasNeverExisted";

        //Act & Assert
        var ex = Assert.Throws<FileNotFoundException>(
            () => builder.AddAppSettingsJsonFile());
    }
}
