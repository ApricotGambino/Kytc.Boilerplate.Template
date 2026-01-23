namespace Api.UnitTests.ConfigurationTests;

using Api.Configurations;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.Configuration;
using NUnit.Framework;
using TestShared.Fixtures;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class WebApplicationBuilderConfigurationTests : BaseTestFixture
{
    #region AddAppSettingsJsonFile
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
    public async Task AddAppSettingsClassBinding_BoundAppSettingDOTJson_AppSettingClassIsPopulated()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.EnvironmentName;

        //Act
        builder.AddAppSettingsJsonFile();
        builder.AddAppSettingsClassBinding();
        var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        //Act
        Assert.That(appSettings, Is.Not.Null);
        Assert.That(appSettings.ConnectionStrings.DefaultConnection, Is.EqualTo("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template.UnitTest;Trusted_Connection=True;MultipleActiveResultSets=true"));
    }
    #endregion

    #region GetAppSetting
    [Test]
    public async Task GetAppSettings_BoundAppSettingDOTJson_ReturnsCorrectValue()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.EnvironmentName;

        //Act
        builder.AddAppSettingsJsonFile();
        builder.AddAppSettingsClassBinding();
        var appSettings = builder.GetAppSettings();

        //Act
        Assert.That(appSettings, Is.Not.Null);
        Assert.That(appSettings.ConnectionStrings.DefaultConnection, Is.EqualTo("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template.UnitTest;Trusted_Connection=True;MultipleActiveResultSets=true"));
    }

    [Test]
    public async Task GetAppSettings_RemoveAllConfigurationSourcesSoAppSettingObjectCannotBeBound_ThrowsError()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.Sources.Clear();

        //Act & Assert
        Assert.Throws<InvalidConfigurationException>(
            () => builder.GetAppSettings());
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
    #endregion

    #region AddDbContext

    [Test]
    public async Task AddDbContext_UnitTestAppSetting_DatabaseContextHasCorrectConnectionString()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.EnvironmentName;
        builder.AddAppSettingsJsonFile();
        builder.AddAppSettingsClassBinding();
        var appSettings = builder.GetAppSettings();


        //Act
        builder.AddDbContext(appSettings);
        var dbContext = builder.Services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();
        var databaseConnection = dbContext.Database.GetDbConnection();

        //Act
        Assert.That(databaseConnection.Database, Is.EqualTo(TestingConstants.UnitTestDatabaseName));
    }

    [Test]
    public async Task AddDbContext_IntentionallyBadUnitTestTestAppSetting_DatabaseContextHasCorrectConnectionStringAndNotDefaultValue()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.IntentionallyBadEnvironmentName;
        builder.AddAppSettingsJsonFile();
        builder.AddAppSettingsClassBinding();
        var appSettings = builder.GetAppSettings();


        //Act
        builder.AddDbContext(appSettings);
        var dbContext = builder.Services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();
        var databaseConnection = dbContext.Database.GetDbConnection();

        //Act
        Assert.That(databaseConnection.Database, Is.Not.EqualTo(TestingConstants.UnitTestDatabaseName));
    }
    #endregion
}
