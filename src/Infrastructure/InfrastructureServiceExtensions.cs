// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager config,
        ILogger logger)
    {
        //string? connectionString = config.GetConnectionString("SqliteConnection");
        var connectionString = config.GetConnectionString("DefaultConnection");
        //Guard.Against.Null(connectionString, message: "Connection string 'CleanArchitectureDb' not found.");

        //services.AddDbContext<ApplicationDbContext>((sp, options) =>
        //{
        //    //options.UseSqlServer(connectionString);//.AddAsyncSeeding(sp);
        //});


        //Guard.Against.Null(connectionString);
        //services.AddDbContext<ApplicationDbContext>(options =>
        // options.UseSqlite(connectionString));

        //services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
        //       .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
        //       .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
        //       .AddScoped<IDeleteContributorService, DeleteContributorService>();


        //logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }
}
