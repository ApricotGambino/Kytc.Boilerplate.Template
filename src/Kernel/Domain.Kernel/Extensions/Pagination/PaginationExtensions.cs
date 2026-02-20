namespace KernelData.Extensions.Pagination;

using System;
using Microsoft.EntityFrameworkCore;

public static class PaginationExtensions
{

    /// <summary>
    /// Gets a paginated set of results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static async Task<PagedResults<T>> ToPaginatedResultsAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(query));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pageNumber, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        var results = await query.ToAsyncEnumerable().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var totalItems = await query.ToAsyncEnumerable().CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedResults<T>
        {
            Results = results,
            PageSize = pageSize,
            Page = pageNumber,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
}
