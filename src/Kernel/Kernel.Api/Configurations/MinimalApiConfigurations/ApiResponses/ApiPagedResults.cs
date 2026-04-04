// ApiPagedResults.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository.

using Kernel.Data.Mapping;
using Kernel.Infrastructure.Extensions.Pagination;

namespace Kernel.Api.Configurations.MinimalApiConfigurations.ApiResponses;

/// <summary>
/// Object that represents paginated results
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiPagedResults<TDto, T> : PagedResults<T>, IDto, IMap<PagedResults<T>, ApiPagedResults<TDto>>
    where TDto : IDto
{
    public static ApiResponses.ApiPagedResults<TDto> MapFrom(PagedResults<T> entity) => throw new NotImplementedException();
}

public class ApiPagedResults<T> : PagedResults<T>
{
}
