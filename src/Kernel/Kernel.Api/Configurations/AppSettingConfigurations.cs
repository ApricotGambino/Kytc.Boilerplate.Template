namespace Kernel.Api.Configurations;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.Configuration;

public static class AppSettingConfigurations
{
    /// <summary>
    /// Adds AppSetting functionality 
    /// </summary>
    /// <typeparam name="TAppSettings"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddAppSettings<TAppSettings>(this WebApplicationBuilder builder)
        where TAppSettings : BaseAppSettings
    {
        builder.AddAppSettingsJsonFile();
        builder.AddAppSettingsClassBinding<TAppSettings>();
        return builder;
    }

    /// <summary>
    /// Extension method that adds the appsetting JSON files. 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static WebApplicationBuilder AddAppSettingsJsonFile(this WebApplicationBuilder builder)
    {
        //TODO: Is this really needed?
        builder.Configuration.AddJsonFile("appsettings.json", optional: false);

        //NOTE: This is only used for local development
        //TODO: Explain that this is set from Api/Properties/launchSettings
        //TODO: Also test to see if we can run the application as a 'unit test user' 
        if (builder.Environment.EnvironmentName != "Local")
        {
            //Override the default appsettings with the environment version only if we're not running the 'local' version.
            builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
        }

        return builder;
    }

    /// <summary>
    /// Add the 'options pattern' to map the appsetting.json file to the AppSetting.cs class. 
    /// </summary>
    /// <typeparam name="TAppSettings"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static WebApplicationBuilder AddAppSettingsClassBinding<TAppSettings>(this WebApplicationBuilder builder)
        where TAppSettings : BaseAppSettings
    {
        builder.Services.Configure<TAppSettings>(builder.Configuration.GetSection("AppSettings"));

        return builder;
    }

    /// <summary>
    /// Gets the AppSettings object configured and populated with values from appsettings.json
    /// </summary>
    /// <typeparam name="TAppSettings"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="InvalidConfigurationException"></exception>
    public static TAppSettings GetAppSettings<TAppSettings>(this WebApplicationBuilder builder)
        where TAppSettings : BaseAppSettings
    {
        //builder.Services.Configure<TAppSettings>(builder.Configuration.GetSection("AppSettings"));
        var appSettings = builder.Configuration.GetSection("AppSettings").Get<TAppSettings>();

        if (appSettings == null)
        {
            throw new InvalidConfigurationException($"Could not get an {nameof(TAppSettings)} object, is this object configured correctly and the appsettings.json file valid and formatted as expected?");
        }

        return appSettings;
    }
}
