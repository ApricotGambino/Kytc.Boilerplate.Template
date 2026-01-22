namespace Api.UnitTests.ConfigurationTests;

using Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class WebApplicationBuilderConfigurationTests
{
    [Test]
    public async Task AddAppSettingsJsonFile_UsingUnitTestEnvironment_FindsExpectedValue()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.EnvironmentName;

        //Act
        builder.AddAppSettingsJsonFile();
        var configuration = builder.Configuration.GetSection(TestingConstants.AppSettingKey).Value;

        //Act
        Assert.That(configuration, Is.EqualTo(TestingConstants.AppSettingValue));
    }

    [Test]
    public async Task AddAppSettingsJsonFile_UsingIntentionallyBadUnitTestEnvironment_DoesNotFindValueOnlyInUnitTestEnvironment()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.IntentionallyBadEnvironmentName;

        //Act
        builder.AddAppSettingsJsonFile();
        var configuration = builder.Configuration.GetSection(TestingConstants.AppSettingKey).Value;

        //Act
        Assert.That(configuration, Is.Null);
    }

    [Test]
    public async Task AddAppSettingsJsonFile_MissingAppSettingFile_ThrowsException()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = "EnvironmentThatDoesNotExist";

        //Act & Assert
        var ex = Assert.Throws<FileNotFoundException>(
            () => builder.AddAppSettingsJsonFile());
    }

    [Test]
    public async Task AddAppSettingsClassBinding_BoundAppSettingDOTJson_AppSettingClassIsPopulated()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = "UnitTest";

        //Act
        builder.AddAppSettingsJsonFile();
        builder.AddAppSettingsClassBinding();
        var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        //Act
        Assert.That(appSettings, Is.Not.Null);
        Assert.That(appSettings.ConnectionStrings.DefaultConnection, Is.EqualTo("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template.UnitTest;Trusted_Connection=True;MultipleActiveResultSets=true"));
    }

    [Test]
    public async Task GetAppSettings_BoundAppSettingDOTJson_ReturnsCorrectValue()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = "UnitTest";

        //Act
        builder.AddAppSettingsJsonFile();
        builder.AddAppSettingsClassBinding();
        var appSettings = builder.GetAppSettings();

        //Act
        Assert.That(appSettings, Is.Not.Null);
        Assert.That(appSettings.ConnectionStrings.DefaultConnection, Is.EqualTo("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template.UnitTest;Trusted_Connection=True;MultipleActiveResultSets=true"));
    }

    [Test]
    public async Task GetAppSettings_IntentionallyBadUnitTestBoundAppSettingDOTJson_CorrectAppSettingIsNotBound()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = "IntentionallyBadUnitTest";

        //Act
        builder.AddAppSettingsJsonFile();
        builder.AddAppSettingsClassBinding();
        var appSettings = builder.GetAppSettings();

        //Act
        Assert.That(appSettings, Is.Not.Null);
        Assert.That(appSettings.ConnectionStrings.DefaultConnection, Is.Not.EqualTo("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template.UnitTest;Trusted_Connection=True;MultipleActiveResultSets=true"));
        Assert.That(appSettings.ConnectionStrings.DefaultConnection, Is.EqualTo("Server=(localdb)\\mssqllocaldb;Database=ThisDatabaseShouldntExistAndShouldFailTesting;Trusted_Connection=True;MultipleActiveResultSets=true"));
    }




}
