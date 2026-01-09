using Api;
using Api.Configurations;
using Microsoft.Extensions.Options;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddAppSettings(); //Get the correct appsettings

    //Configure the appsettings in the IOptions pattern, converting the appsettings.json to a hardened AppSettings object. 
    var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

    if (appSettings == null)
    {
        //TODO: Consider using a guard here instead. 
        throw new Exception("AppSettings could not be loaded correctly.");
    }

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
}
finally
{
    Log.CloseAndFlush();
}
