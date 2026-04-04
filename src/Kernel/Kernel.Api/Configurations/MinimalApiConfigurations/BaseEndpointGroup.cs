// BaseEndpointGroup.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository.

using Microsoft.AspNetCore.Routing;

namespace Kernel.Api.Configurations.MinimalApiConfigurations;

//TODO: Document this.
public abstract class BaseEndpointGroup
{
    public virtual string? GroupName { get; }
    public abstract void Map(RouteGroupBuilder groupBuilder);
}



