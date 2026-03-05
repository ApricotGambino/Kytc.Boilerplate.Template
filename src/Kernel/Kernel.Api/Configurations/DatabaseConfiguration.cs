// DatabaseConfiguration.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Kernel.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Kernel.Api.Configurations;

public static class DatabaseConfiguration
{
    /// <summary>
    /// Adds the DB context with configurations
    /// </summary>
    /// <remarks>This is to be called prior to builder.Build()</remarks>
    /// <typeparam name="TDatabaseContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="appSettings"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDbContextConfiguration<TDatabaseContext>(this WebApplicationBuilder builder, BaseAppSettings appSettings)
    where TDatabaseContext : DbContext
    {
        builder.Services.AddDbContext<TDatabaseContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);
        });

        builder.Services.AddScoped(typeof(ReadOnlyEntityRepo<,>));

        return builder;
    }

    /// <summary>
    /// This is responsible for configuring the database with startup actions
    /// </summary>
    /// <returns>This is to be called after builder.Build()</returns>
    /// <param name="app"></param>
    /// <param name="configuration"></param>
    public static void AddDbContextStartupActions<TDatabaseContext>(this WebApplication app, BaseAppSettings appSettings, IServiceProvider serviceProvider)
        where TDatabaseContext : DbContext
    {

        if (appSettings.EnableAutomaticMigrations)
        {
            ApplyMigrations<TDatabaseContext>(serviceProvider);
        }
        //using (var scope = app.ApplicationServices.CreateScope())
        //{
        //    ApplicationFeatureFlags applicationFeatureFlags = configuration.GetSection(nameof(ApplicationFeatureFlags)).Get<ApplicationFeatureFlags>();
        //    ConnectionStrings connectionStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();

        //    if (applicationFeatureFlags.EnableAutomaticMigrations)
        //    {
        //        ApplyMigrations(scope.ServiceProvider);
        //    }

        //    if (applicationFeatureFlags.EnableSeeding)
        //    {
        //        SeedData(scope.ServiceProvider);
        //    }

        //    SetupBackgroundJobs(scope.ServiceProvider);
        //}
    }

    /// <summary>
    /// This will execute any missing migrations to the database
    /// </summary>
    /// <typeparam name="TDatabaseContext"></typeparam>
    /// <param name="services"></param>
    /// <exception cref="Exception"></exception>
    private static void ApplyMigrations<TDatabaseContext>(IServiceProvider services)
        where TDatabaseContext : DbContext
    {






        //Add: dotnet ef --verbose --project src/Data/Data.csproj --startup-project src/Api/Api.csproj migrations add Initial -- --environment Local
        //Update: dotnet ef --verbose --project src/Data/Data.csproj --startup-project src/Api/Api.csproj database update -- --environment Local

        //TODO: Document and unit test this.
        var context = services.GetRequiredService<TDatabaseContext>();

        if (!context.Database.GetMigrations().Any())
        {
            //TODO: Update this message.
            throw new Exception("No migrations found, add an initial migration using 'Add-Migration Initial -Context ApplicationDbContext'.  " +
                "If using Visual Studio, set the Web project as the startup one, and in the package manager console, select the DataAccess project in the 'default projects' section.");
        }

        context.Database.Migrate();
    }
}
