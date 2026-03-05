// MinimalApiConfigurations.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Kernel.Api.Configurations.MinimalApiConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Kernel.Api.Configurations.MinimalApiConfigurations;

public static class MinimalApiConfigurations
{
    /// <summary>
    /// Creates a <see cref="RouteGroupBuilder"/> based on the provided <see cref="BaseEndpointGroup"/>
    /// </summary>
    /// <param name="app"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    private static RouteGroupBuilder MapGroup(this WebApplication app, BaseEndpointGroup group)
    {
        var groupName = group.GroupName ?? group.GetType().Name;

        return app
            .MapGroup($"/api/{groupName}");
    }


    /// <summary>
    /// This method finds all exposed methods in any endpoint group which inherit from <see cref="BaseEndpointGroup"/>
    /// and creates and maps a RouteGroupBuilder to the endpoint.
    /// </summary>
    /// <remarks>
    /// EX: Will create and map LoggingEndpoint.GetLogs() to /api/LoggingEndpoint/
    /// </remarks>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointGroupType = typeof(BaseEndpointGroup);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var endpointGroupTypes = assemblies.SelectMany(s => s.GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType)));

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is BaseEndpointGroup instance)
            {
                instance.Map(app.MapGroup(instance));
            }
        }

        return app;
    }
}

