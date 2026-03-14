// PaginationExtensions.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Microsoft.EntityFrameworkCore;

namespace Kernel.Infrastructure.Extensions.Pagination;

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

    public static PagedResults<TDto> ToMappedPagedResults<TEntity, TDto>(this PagedResults<TEntity> query)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(query));
        //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pageNumber, 0);
        //ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        //var results = await query.ToAsyncEnumerable().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        //var totalItems = await query.ToAsyncEnumerable().CountAsync();
        //var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedResults<TDto>
        {
            //Results = results,
            //PageSize = pageSize,
            //Page = pageNumber,
            //TotalItems = totalItems,
            //TotalPages = totalPages
        };
    }
}

public static class GenericCollectionExtensions
{
    public sealed class CollectionConverter<T>
    {
        private readonly PagedResults<T> _source;

        public CollectionConverter(PagedResults<T> source)
        {
            _source = source;
        }

        public PagedResults<TTo> To<TTo>()
        //where TCollection : ICollection<T>, new()
        where TTo : IMap<T, TTo>
        {
            var convertedList = new List<TTo>();

            if (_source.Results != null)
            {
                foreach (var item in _source.Results)
                {
                    convertedList.Add(TTo.MapFrom(item));
                }
            }

            var collection = new PagedResults<TTo>()
            {
                Page = _source.Page,
                TotalItems = _source.TotalItems,
                TotalPages = _source.TotalPages,
                PageSize = _source.PageSize,
                Results = convertedList
            };

            return collection;
        }
    }

    //TODO: Rename this.
    public static CollectionConverter<T> Convert<T>(this PagedResults<T> pagedResults)
    {
        return new CollectionConverter<T>(pagedResults);
    }
}


//TODO: Put this somewhere it should go.
public interface IMap<in TFrom, out TTo>
{
    static abstract TTo MapFrom(TFrom entity);
}
