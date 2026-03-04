using Api;
using Data.EntityFramework;
using Kernel.Api;
using Microsoft.Extensions.Options;
using Serilog;

//NOTE:
//This is the entry point for your application's API.  Common functionality is isolated to the
//Kernel section of the source code.  This endpoint should call the Kernel base configuration,
//and only configururations specific to your application.
try
{
    var builder = BaseWebApplicationBuilder.CreateBaseWebApplicationBuilder<ApplicationDbContext, AppSettings>(args);

    //builder.Services.AddScoped<IExampleService, ExampleService>();

    var app = builder.Build();

    //NOTE: Not using builder.GetAppSettings<AppSettings>() and instead resolving the IOptions DI for AppSettings
    //because at this point in the pipeline we can modify those appsettings (Such as dynamically modifying the appsettings based on logic, or unit testing)
    //whereas with the builder.GetAppSettings<AppSettings>() example
    //that is ONLY driven by the Configuration object which draws from the actual appsetting.json file.
    var appSettings = app.Services.GetService<IOptions<AppSettings>>()!.Value;

    app.AddKernelWebApplicationConfigurations(appSettings);

    app.MapGet("/appsettings", () =>
    {
        //Log.Fatal("testthing4");
        return appSettings;
    });

    await app.RunAsync();
}
finally
{
    await Log.CloseAndFlushAsync();
}




