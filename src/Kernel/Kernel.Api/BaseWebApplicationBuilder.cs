namespace Kernel.Api;

using Kernel.Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

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

        builder.AddSerilogConfiguration(appSettings);
        builder.AddDbContextConfiguration<TDatabaseContext>(appSettings);
        builder.AddKernelServices();

        return builder;
    }


}
