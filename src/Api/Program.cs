using Api;
using Api.Configurations;
using Domain.Interfaces.Features.Logging;
using Infrastructure.Features.Logging.Services;
using Microsoft.Extensions.Options;
using Serilog;

//TODO:
//[DONE]Create compiler error for methods that are async to be named _async
//[DONE]Create compiler error for methods that are async to not return void.

//TODO: Use .AsNoTracking for readonly DB. stuff. 
//This would work on a repo class: 
//public IQueryable<T> GetQuery<T>() where T : class
//{
//    return this.Set<T>().AsNoTracking();
//}


//Creating a logging service that writes and reads from the DB.
//  NOTE: This service isn't a replacment for using the .net logging feature, it gets
//  data that we may want specifically for logging on the admin screen.
//Create tests for logging service
//  Check that it works manually.
//  Create an integration test for it.
//  Create a SUT unit test for it.
//  Create a functional test for it. This test will probably break after you do the API format stuff. 
//Create ISaveChangesInterceptor thing
//Create tests for ISaveChangesInterceptor thing
//Create tests for DB context setup
//Create user stuff
//Create db seed
//Create tests for db seed
//Create tests for User stuff
//Create API thing
//Create test for API thing
//Create exception handling for API thing
//Add compiler error for 'black hole' exceptions, no warnings, full stop build failure. 
//Create test for API exception handling thing
//Create permission stuff
//Create tests for permission stuff
//At this point, you should be able to:
//  unit test SUT logging service,
//  integration tests for logging service
//  functional tests as a user with permissions.
//  Run the application
//Mostly be able to write code, THEN test instead of this awkward do-debug-write-test-do.
//Write auto generation for API exposure.
//Code analysis. You're ready to add features.
//Look through both other solutions for goodies to add. 
//About time to work on creating the template proper. 
//  And now it's time to think about how to add these 'BP' files, since they ar ecritical, should they be uh..
//  Put in a way that's easy to know 'dont mess with these?'
//Eventually, you can work on the front end.  



//TODO: Explain all the anlyzizsers I've installed
//  Also, that for VS, it only analysis open files unless you change your setting: 
// https://stackoverflow.com/questions/49592058/roslyn-analyzer-only-runs-for-open-files

//TODO: Make service calls and API calls paginated. Somehow.

try
{
    var builder = WebApplication.CreateBuilder(args);
    //builder.AddAppSettings(); //Get the correct appsettings
    builder.AddAppSettingsJsonFile()
        .AddAppSettingsClassBinding();

    //TODO: is this needed?
    //builder.Configuration.AddEnvironmentVariables(); //Override any appsetting values in the above files with anything configured in the environment. (Secrets)

    //Add the 'options pattern' as a service for DI to fetch in other services. 
    //builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
    //Configure the appsettings in the IOptions pattern, converting the appsettings.json to a hardened AppSettings object. 
    //var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
    var appSettings = builder.GetAppSettings();


    builder
        .AddLoggerConfigs(appSettings)
        .AddDbContext(appSettings);

    //TODO: Figure out where to put service registrations.
    builder.Services.AddScoped<ILoggingService, LoggingService>();

    var app = builder.Build();



    app.UseHttpsRedirection();

    app.MapGet("/appsettings", () =>
    {
        var appSettings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;
        Log.Fatal("testthing4");
        return appSettings;
    });

    await app.RunAsync();
}
catch (Exception ex)
{
    //TODO: Test how exceptions bubble up with this. 
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
//public partial class Program { }
