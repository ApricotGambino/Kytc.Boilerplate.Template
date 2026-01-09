namespace Api.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

//NOTE: WebApplication.CreateBuilder does a lot of things by default: 
//Such as appsetting loading, logging and DI.  But we're going to define those manually regardless. 
//https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.webapplication.createbuilder?view=aspnetcore-9.0

public static class WebApplicationBuilderConfigurations
{
    /// <summary>
    /// Extension method responisble for adding appsetting configurations for the application.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddAppSettings(this WebApplicationBuilder builder)
    {
        //Add the appsettings.json files. 
        builder.AddAppSettingsJsonFile();

        //Add the 'options pattern' as a service for DI to fetch in other services. 
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
        // If you need to access during setup.
        // Here, configuration is ConfigurationManager
        // e.g. via WebApplicationBuilder.Configuration
        //var currentConfig = configuration
        //  .GetSection(nameof(AppConfig))
        //  .Get<AppConfig>();

        builder.Configuration.AddEnvironmentVariables(); //Override any appsetting values in the above files with anything configured in the environment. (Secrets)
        return builder;
    }

    /// <summary>
    /// Extension method that adds the appsetting JSON files. 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static WebApplicationBuilder AddAppSettingsJsonFile(this WebApplicationBuilder builder)
    {

        builder.Configuration.AddJsonFile("appsettings.json", optional: false);
        //TODO: REmove environment variables, we won't use them. 
        if (builder.Environment.EnvironmentName != "Local")
        {
            //Override the default appsettings with the environment version only if we're not running the 'local' version.
            builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false);
        }

        return builder;
    }
    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            //options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template;Trusted_Connection=True;MultipleActiveResultSets=true");
            options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);

            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        return builder;
    }
}

