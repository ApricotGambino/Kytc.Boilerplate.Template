using Application.Features.ExampleFeature.Services;
using Kernel.Api;

namespace Api.Configurations
{
    public static class ApiServiceConfigurations
    {
        /// <summary>
        /// Adds API services.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="appSettings"></param>
        /// <returns></returns>
        internal static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder, BaseAppSettings appSettings)
        {
            builder.Services.AddScoped<IExampleService, ExampleService>();

            return builder;
        }
    }
}
