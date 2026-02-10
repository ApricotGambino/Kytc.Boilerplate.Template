namespace KernelApi;

using KernelApi.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

public static class BaseWebApplicationBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebApplicationBuilder"/> class with preconfigured defaults along with
    /// kernel configurations, along with a Database context.
    /// </summary>
    /// <typeparam name="TDatabaseContext"></typeparam>
    /// <typeparam name="TAppSettings"></typeparam>
    /// <param name="args"></param>
    /// <param name="environmentName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder CreateBaseWebApplicationBuilder<TDatabaseContext, TAppSettings>(string[] args, string environmentName = null)
        where TDatabaseContext : DbContext
         where TAppSettings : BaseAppSettings
    {
        var builder = WebApplication.CreateBuilder(args);

        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            //Normally the environment name is provided by the entity running this code,
            //that might be an Environment defined by a server, or a dockerfile build, using dotnet run -e NameOfEnvironment, or even
            //visual studio's launchSettings.json.  But we expose an easy way to set that through the
            //method signature for unit testing. 
            builder.Environment.EnvironmentName = environmentName;
        }

        builder.AddAppSettings<TAppSettings>();
        var appSettings = builder.GetAppSettings<TAppSettings>();

        builder.AddDbContextConfiguration<TDatabaseContext>(appSettings);
        builder.AddKernelServices(appSettings);

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
        var appSettings = builder.Configuration.GetSection("AppSettings").Get<TAppSettings>();

        if (appSettings == null)
        {
            throw new InvalidConfigurationException($"Could not get an {nameof(TAppSettings)} object, is this object configured correctly and the appsettings.json file valid and formatted as expected?");
        }

        return appSettings;
    }


}
