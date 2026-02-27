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
    public static WebApplication AddKernelWebApplicationConfigurations(this WebApplication app)
    {
        //TODO: Test that HTTPS redirection actually works.
        app.UseHttpsRedirection();
        app.MapEndpoints();


        //TODO: Should this be in development?  Also, test mapopenapi
        //https://localhost:44341/openapi/v1.json
        //if (app.Environment.IsDevelopment())
        //{
        app.MapOpenApi();
        app.MapScalarApiReference();
        //}
        return app;
    }
}
