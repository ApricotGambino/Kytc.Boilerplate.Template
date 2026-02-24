// PagedResults.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

namespace Kernel.Infrastructure.Extensions.Pagination;

/// <summary>
/// Object that represents paginated results
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedResults<T>
{
    /// <summary>
    /// Nullable Collection of items returned by pagination
    /// </summary>
    public List<T>? Results { get; set; }
    /// <summary>
    /// The maximum number of results to be returned by the pagination
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// The current page of results based on query to filter and/or order results
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// The total number of results possible based on the executed query
    /// </summary>
    public int TotalItems { get; set; }
    /// <summary>
    /// The total number of pages based on query and pagesize
    /// </summary>
    public int TotalPages { get; set; }
}
