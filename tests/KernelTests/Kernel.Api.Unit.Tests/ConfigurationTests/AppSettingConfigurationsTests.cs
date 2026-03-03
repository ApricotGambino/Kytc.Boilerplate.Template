using Api;
using Kernel.Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using TestShared.Fixtures;

namespace Kernel.Api.Unit.Tests.ConfigurationTests;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class AppSettingConfigurationsTests : BaseTestFixture
{
    [Test]
    public async Task AddAppSettings_GetValueFromConfiguration_FindsExpectedValue()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.TestingEnvironmentName;

        //Act
        builder.AddAppSettings<AppSettings>();
        var configuration = builder.Configuration.GetSection($"AppSettings:{nameof(TestingConstants.ApplicationName)}").Value;

        //Act
        Assert.That(configuration, Is.EqualTo(TestingConstants.ApplicationName));
    }

    [Test]
    public async Task AddAppSettings_GetAppSettingObject_FindsExpectedValue()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.TestingEnvironmentName;

        //Act
        builder.AddAppSettings<AppSettings>();
        var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

        //Act
        Assert.That(appSettings, Is.Not.Null);
        Assert.That(appSettings.ApplicationName, Is.EqualTo(TestingConstants.ApplicationName));
    }

    [Test]
    public async Task AddAppSettings_MissingAppSettingDOTjson_ThrowsException()
    {

        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.SetBasePath("C:/");

        //Act & Assert
        Assert.Throws<FileNotFoundException>(
            () => builder.AddAppSettings<AppSettings>());
    }

}
