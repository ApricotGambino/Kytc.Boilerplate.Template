// LoggingEndpoint.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Kernel.Api.Configurations.MinimalApiConfigurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Kernel.Api.Endpoints;

public class LoggingEndpoint : BaseEndpointGroup
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        //groupBuilder.RequireAuthorization();

        groupBuilder.MapGet(GetLogs);
    }

    public async Task<Ok<List<string>>> GetLogs()
    {
        //https://localhost:44341/api/LoggingEndpoint/
        var logs = new List<string>() { "A", "B", "C" };

        return TypedResults.Ok(logs);
    }

}
