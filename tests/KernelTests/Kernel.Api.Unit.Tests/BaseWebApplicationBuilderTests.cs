
using Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Protocols.Configuration;
using TestShared.Fixtures;

namespace Kernel.Api.Unit.Tests;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class BaseWebApplicationBuilderTests : BaseTestFixture
{


    [Test]
    public async Task GetAppSettings_GetAppSettingObject_FindsExpectedValue()
    {

        //Arrange
        var args = new string[] { "--environment=Testing" };
        var builder = BaseWebApplicationBuilder.CreateBaseWebApplicationBuilder<TestingDatabaseContext, AppSettings>(args);

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
