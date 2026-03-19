// ApiPagedResults.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Kernel.Data.Mapping;
using Kernel.Infrastructure.Extensions.Pagination;

namespace Kernel.Api.Configurations.MinimalApiConfigurations.ApiResponses;

/// <summary>
/// Object that represents paginated results
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiPagedResults<T> : PagedResults<T>, IDto
    where T : IDto
{
}
