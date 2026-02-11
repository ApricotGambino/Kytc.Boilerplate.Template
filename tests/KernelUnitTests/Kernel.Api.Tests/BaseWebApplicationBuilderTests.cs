namespace KernelApi.Tests;

using Api;
using KernelApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Protocols.Configuration;
using TestShared;
using TestShared.Fixtures;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class BaseWebApplicationBuilderTests : BaseTestFixture
{

    [Test]
    public async Task GetAppSettings_GetAppSettingObject_FindsExpectedValue()
    {
        //Arrange
        var builder = BaseWebApplicationBuilder.CreateBaseWebApplicationBuilder<TestingDatabaseContext, AppSettings>(new string[] { "--environment=Testing" });

        //Act
        var appSettings = builder.GetAppSettings<TestAppSettings>();

        //Act
        Assert.That(appSettings.TestKey, Is.EqualTo(TestingConstants.TestingAppSettingValue));
    }

    [Test]
    public async Task GetAppSettings_UsingIncorrectlyFormattedAppSettings_ThrowsError()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.Sources.Clear();
        //Act & Assert
        Assert.Throws<InvalidConfigurationException>(
            () => builder.GetAppSettings<TestAppSettings>());
    }

}
