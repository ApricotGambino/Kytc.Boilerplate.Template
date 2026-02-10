namespace KernelApi.Configurations;

using System.Globalization;
using KernelInfrastructure.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

internal static class KernelServiceConfigurations
{
    /// <summary>
    /// Adds services defined by the kernel, to the kernel. 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="appSettings"></param>
    /// <returns></returns>
    internal static WebApplicationBuilder AddKernelServices(this WebApplicationBuilder builder, BaseAppSettings appSettings)
    {
        builder.AddSerilogConfiguration(appSettings);
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();


        return builder;
    }

    /// <summary>
    /// Adds and configures Serilog
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="appSettings"></param>
    /// <returns></returns>
    internal static WebApplicationBuilder AddSerilogConfiguration(this WebApplicationBuilder builder, BaseAppSettings appSettings)
    {
        //TODO: create some tests around Serilog, and logging. Also, tidy this up along with feature enrich.
        _ = builder.Services.AddSerilog((services, loggerConfiguration) =>
        loggerConfiguration
            //.ReadFrom.Configuration(builder.Configuration) //TODO: What is this for?
            //.ReadFrom.Services(services) //TODO: What is this for?
            .WriteTo
                .MSSqlServer(
                    connectionString: appSettings.ConnectionStrings.DefaultConnection,
                    //sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", SchemaName = "dbo" },
                    formatProvider: new CultureInfo("en-US")
                    )
            //.MinimumLevel.Warning()
            //.Enrich.FromLogContext() //TODO: What is this for?
            );


        //This ensures that the logs get flushed into the DB no matter what, since MSSQL sink is a 'perodic batch' sink. 
        AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();


        //NOTE: You can uncomment this to debug Serilog itself. 
        //Serilog.Debugging.SelfLog.Enable(msg =>
        //{
        //    Debug.Print(msg);
        //    Debugger.Break();
        //});

        return builder;
    }
}
