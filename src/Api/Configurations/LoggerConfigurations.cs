namespace Api.Configurations;

using System;
using System.Globalization;
using Serilog;
using Serilog.Sinks.MSSqlServer;

public static class LoggerConfigurations
{

    public static WebApplicationBuilder AddLoggerConfigs(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        _ = builder.Services.AddSerilog((services, loggerConfiguration) =>
        loggerConfiguration
            //.ReadFrom.Configuration(builder.Configuration) //TODO: What is this for?
            //.ReadFrom.Services(services) //TODO: What is this for?
            .WriteTo
                .MSSqlServer(
                    connectionString: appSettings.ConnectionStrings.DefaultConnection,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", SchemaName = "dbo" },
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
