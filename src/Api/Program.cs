using Api;
using Data.EntityFramework;
using Kernel.Api;
using Serilog;

//NOTE:
//This is the entry point for your application's API.  Common functionality is isolated to the
//Kernel section of the source code.  This endpoint should call the Kernel base configuration,
//and only configururations specific to your application.
try
{
    var builder = BaseWebApplicationBuilder.CreateBaseWebApplicationBuilder<ApplicationDbContext, AppSettings>(args);

    var appSettings = builder.GetAppSettings<AppSettings>();

    //builder.Services.AddScoped<IExampleService, ExampleService>();

    var app = builder.Build();

    app.AddKernelWebApplicationConfigurations();

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




