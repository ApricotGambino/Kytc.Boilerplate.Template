using Api;
using Application.Features.ExampleFeature.Services;
using Data.EntityFramework;
using KernelApi;
using Microsoft.EntityFrameworkCore;
using Serilog;

//NOTE:
//This is the entry point for your application's API.  Common functionality is isolated to the
//Kernel section of the source code.  This endpoint should call the Kernel base configuration,
//and only configururations specific to your application. 

try
{
    var builder = BaseWebApplicationBuilder.CreateBaseWebApplicationBuilder<ApplicationDbContext, AppSettings>(args);

    var appSettings = builder.GetAppSettings<AppSettings>();

    builder.Services.AddScoped<IExampleService, ExampleService>();

    var app = builder.Build();

    app.AddKernelWebApplicationConfigurations();

    //TODO: Is this a replacement for Guards?
    //ArgumentNullException.ThrowIfNull(app);

    app.MapGet("/appsettings", () =>
    {
        //Log.Fatal("testthing4");
        return appSettings;
    });

    await app.RunAsync();
}
catch (Exception ex)
{
    //TODO: Test how exceptions bubble up with this. 
    //Log.Fatal(ex, "Application terminated unexpectedly");
    var a = 1;
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}




