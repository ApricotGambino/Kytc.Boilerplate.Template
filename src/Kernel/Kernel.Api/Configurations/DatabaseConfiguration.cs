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
}
