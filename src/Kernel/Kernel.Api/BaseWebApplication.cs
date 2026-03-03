// BaseWebApplication.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Kernel.Api.Configurations.MinimalApiConfigurations;
using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;

namespace Kernel.Api;

public static class BaseWebApplication
{
    /// <summary>
    /// This adds all kernel required web application configurations to the webapplication object.
    /// </summary>
    /// <param name="app"></param>
    public static WebApplication AddKernelWebApplicationConfigurations(this WebApplication app, BaseAppSettings baseAppSettings)
    {
        //We always assume our applications to be hosted under HTTPS.
        //We can prove this works by taking the configured launchsettings.json applicationUrls: https://localhost:7236;http://localhost:5031
        //Attempting to access http://localhost:5031 will result in being immediately redirected to https://localhost:7236
        app.UseHttpsRedirection();


        app.MapEndpoints();

        if (baseAppSettings.EnableApiDiscovery)
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options => options.DisableAgent());
        }

        return app;
    }
}
