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
        builder.AddAppSettings<BaseAppSettings>();
        var configuration = builder.Configuration.GetSection($"AppSettings:{TestingConstants.TestingAppSettingKey}").Value;

        //Act
        Assert.That(configuration, Is.EqualTo(TestingConstants.TestingAppSettingValue));
    }

    [Test]
    public async Task AddAppSettings_GetAppSettingObject_FindsExpectedValue()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = TestingConstants.TestingEnvironmentName;

        //Act
        builder.AddAppSettings<TestAppSettings>();
        var appSettings = builder.Configuration.GetSection("AppSettings").Get<TestAppSettings>();

        //Act
        Assert.That(appSettings, Is.Not.Null);
        Assert.That(appSettings.TestKey, Is.EqualTo(TestingConstants.TestingAppSettingValue));
    }

    [Test]
    public async Task AddAppSettings_NoProvidedEnvironmentName_BaseAppSettingIsNotOverriden()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();

        //Act
        builder.AddAppSettings<BaseAppSettings>();
        var applicationNameConfiguration = builder.Configuration.GetSection($"AppSettings:{TestingConstants.ApplicationName}").Value;
        var testingAppSettingKeyConfiguration = builder.Configuration.GetSection($"AppSettings:{TestingConstants.TestingAppSettingKey}").Value;
        using (Assert.EnterMultipleScope())
        {

            //Act
            Assert.That(applicationNameConfiguration, Is.Not.EqualTo(TestingConstants.ApplicationName));
            Assert.That(testingAppSettingKeyConfiguration, Is.Null);
        }
    }

    [Test]
    public async Task AddAppSettings_MissingAppSettingDOTjson_ThrowsException()
    {

        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.SetBasePath("C:/");

        //Act & Assert
        Assert.Throws<FileNotFoundException>(
            () => builder.AddAppSettings<TestAppSettings>());
    }

}
