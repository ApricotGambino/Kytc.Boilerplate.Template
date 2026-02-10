namespace Kernel.Api.Configurations;

using Kernel.Infrastructure.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

public static class KernelServiceConfigurations
{
    /// <summary>
    /// Adds services defined by the kernel, to the kernel. 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddKernelServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        return builder;
    }
}
