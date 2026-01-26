namespace Api.Configurations;

using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

//NOTE: WebApplication.CreateBuilder does a lot of things by default: 
//Such as appsetting loading, logging and DI.  But we're going to define those manually regardless. 
//https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.webapplication.createbuilder?view=aspnetcore-9.0

public static class WebApplicationBuilderConfigurations
{
    ///// <summary>
    ///// Extension method responisble for adding appsetting configurations for the application.
    ///// </summary>
    ///// <param name="builder"></param>
    ///// <returns></returns>
    //public static WebApplicationBuilder AddAppSettings(this WebApplicationBuilder builder)
    //{
    //    //Add the appsettings.json files. 
    //    builder.AddAppSettingsJsonFile();

    //    builder.Configuration.AddEnvironmentVariables(); //Override any appsetting values in the above files with anything configured in the environment. (Secrets)

    //    //Add the 'options pattern' as a service for DI to fetch in other services. 
    //    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
    //    // If you need to access during setup.
    //    // Here, configuration is ConfigurationManager
    //    // e.g. via WebApplicationBuilder.Configuration
    //    //var currentConfig = configuration
    //    //  .GetSection(nameof(AppConfig))
    //    //  .Get<AppConfig>();

    //    return builder;
    //}

    /// <summary>
    /// Extension method that adds the appsetting JSON files. 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddAppSettingsJsonFile(this WebApplicationBuilder builder)
    {
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
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddAppSettingsClassBinding(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

        return builder;
    }

    /// <summary>
    /// Returns the bound AppSettings class instance.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="InvalidConfigurationException"></exception>
    public static AppSettings GetAppSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
        var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        if (appSettings == null)
        {
            throw new InvalidConfigurationException("Could not get an AppSetting object from the appsettings.json, is the appsetting.json file formatted correctly?");
        }

        return appSettings;
    }

    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            //TODO: options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);
        });

        return builder;
    }
}

