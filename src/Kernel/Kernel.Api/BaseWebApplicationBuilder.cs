// BaseWebApplicationBuilder.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning


using KernelApi.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace KernelApi;

public static class BaseWebApplicationBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebApplicationBuilder"/> class with preconfigured defaults along with
    /// kernel configurations, along with a Database context.
    /// </summary>
    /// <typeparam name="TDatabaseContext"></typeparam>
    /// <typeparam name="TAppSettings"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    public static WebApplicationBuilder CreateBaseWebApplicationBuilder<TDatabaseContext, TAppSettings>(string[] args)
        where TDatabaseContext : DbContext
         where TAppSettings : BaseAppSettings
    {
        var builder = WebApplication.CreateBuilder(args);

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
