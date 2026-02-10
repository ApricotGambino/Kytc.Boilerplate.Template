namespace KernelApi;

using Microsoft.AspNetCore.Builder;

public static class BaseWebApplication
{
    /// <summary>
    /// This adds all kernel required web application configurations to the webapplication object.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication AddKernelWebApplicationConfigurations(this WebApplication app)
    {
        //TODO: Test that HTTPS redirection actually works. 
        app.UseHttpsRedirection();

        return app;
    }
}
