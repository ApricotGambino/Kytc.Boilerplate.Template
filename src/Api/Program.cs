using Api;
using Api.Configurations;
using Data.EntityFramework;
using Kernel.Api;
using Microsoft.Extensions.Options;
using Serilog;

//NOTE:
//This is the entry point for your application's API.  Common functionality is isolated to the
//Kernel section of the source code.  This should call the Kernel base configuration,
//and only configururations specific to your application.
try
{
    var builder = BaseWebApplicationBuilder.CreateBaseWebApplicationBuilder<ApplicationDbContext, AppSettings>(args);

    //NOTE: At this point in the pipeline, we have to use the appsettings from the actual appsettings.json file, we don't have
    //access to the Ioptions version that will later be injected through dependency injection.
    var appSettingsFromFile = builder.GetAppSettings<AppSettings>();

    builder.AddApiServices(appSettingsFromFile);

    //NOTE: This shows endpoint binding errors.
    builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = true);

    //NOTE: This is the point at which we've configured services, and after things are built, we'll be able to use those services.
    var app = builder.Build();

    var appServiceProvider = app.Services;

    //NOTE: Now that the app has been built, we have access to the injected version  of the appsettings.
    //Prior to builder.Build(), we're creating the services and could only read from a file, but now we're configuring them, and as such, we
    //have the ability to have those appsettings modified or adjusted (Such as dynamically modifying the appsettings based on logic, or unit testing).
    //It's unlikely the values from the file to the values from the injected will be different outside of unit testing, but since the
    //rest of the application will read from the Ioptions version, we'll use that now that we have access to it.
    var appSettingsFromService = appServiceProvider.GetService<IOptions<AppSettings>>()!.Value;

    app.AddKernelWebApplicationConfigurations<ApplicationDbContext>(appSettingsFromService, appServiceProvider);

    app.MapGet("/appsettings", () =>
    {
        //Log.Fatal("testthing4");
        return appSettingsFromService;
    });


    await app.RunAsync();
}
finally
{
    await Log.CloseAndFlushAsync();
}




