// MinimalApiConfigurations.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Reflection;
using Kernel.Api.Configurations.MinimalApiConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Kernel.Api.Configurations.MinimalApiConfigurations;

public static class MinimalApiConfigurations
{
    private static RouteGroupBuilder MapGroup(this WebApplication app, BaseEndpointGroup group)
    {
        var groupName = group.GroupName ?? group.GetType().Name;

        return app
            .MapGroup($"/api/{groupName}");
        //.WithGroupName(groupName);
        //.WithTags(groupName); TODO: This is for swagger, test this.
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointGroupType = typeof(BaseEndpointGroup);

        var assembly = Assembly.GetExecutingAssembly();

        var endpointGroupTypes = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType));

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

