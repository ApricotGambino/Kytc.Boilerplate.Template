namespace Api.Configurations;
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
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

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
        if (builder.Environment.EnvironmentName != "Local")
        {
            //Override the default appsettings with the environment version only if we're not running the 'local' version.
            builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false);
        }

        return builder;
    }
}

