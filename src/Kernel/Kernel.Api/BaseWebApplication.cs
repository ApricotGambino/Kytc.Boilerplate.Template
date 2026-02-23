// BaseWebApplication.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Microsoft.AspNetCore.Builder;

namespace KernelApi;

public static class BaseWebApplication
{
    /// <summary>
    /// This adds all kernel required web application configurations to the webapplication object.
    /// </summary>
    /// <param name="app"></param>
    public static WebApplication AddKernelWebApplicationConfigurations(this WebApplication app)
    {
        //TODO: Test that HTTPS redirection actually works.
        app.UseHttpsRedirection();

        return app;
    }
}
