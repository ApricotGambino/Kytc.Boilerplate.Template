using Api;
using Api.Configurations;
using Microsoft.Extensions.Options;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    //builder.AddAppSettings(); //Get the correct appsettings
    builder.AddAppSettingsJsonFile();

    //TODO: is this needed?
    builder.Configuration.AddEnvironmentVariables(); //Override any appsetting values in the above files with anything configured in the environment. (Secrets)

    //Add the 'options pattern' as a service for DI to fetch in other services. 
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

    //Configure the appsettings in the IOptions pattern, converting the appsettings.json to a hardened AppSettings object. 
    var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();


    builder
        .AddLoggerConfigs(appSettings)
        .AddDbContext(appSettings);

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.MapGet("/appsettings", () =>
    {
        var appSettings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;
        Log.Fatal("testthing4");
        return appSettings;
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
public partial class Program { }
