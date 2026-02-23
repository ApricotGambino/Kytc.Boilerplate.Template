// ReadOnlyEntityRepo.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning


using System.Linq.Expressions;
using KernelData.Entities;
using KernelData.EntityFramework;
using KernelData.Extensions.Pagination;
using Microsoft.EntityFrameworkCore;

//This file is
//NOTE: asdfasdf
//! Big test!
//? What?
//TODO: Thing
//Use '?' for Question.
//Use "Todo" (Case ignored) for Task.
////Use 'x', 'X', or double comment for strikethrough (Crossed).

namespace KernelInfrastructure.Repositories;
//NOTE: There's a much more indepth discussion around why you should use this found in the Sandbox/Concept Demos/HowDoIGetData/HowDoIGetDataTests.cs file.



/// <summary>
/// This repository fetches data using .AsNoTracking() which ensures entities aren't being tracked by EF.
/// </summary>
/// <remarks>///
/// This repository is meant to be used with the idea of pagination in mind, and is intentionally limited in the API for that reason.
/// If you absolutely must fetch large volumes of data that probably couldn't be displayed on a page, then you can use <see cref="GetEntityQueryable"/>///
/// Pagination must always use ordering, if never applied, the default order will be by ID ascending, which means the first page will be the oldest records.
/// </remarks>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDatabaseContext"></typeparam>
/// <param name="context"></param>
public class ReadOnlyEntityRepo<TEntity, TDatabaseContext>(TDatabaseContext context)
    where TEntity : BaseEntity
    where TDatabaseContext : BaseDbContext
{
    protected TDatabaseContext Context { get; set; } = context;

    /// <summary>
    /// Gets the an <see cref="IQueryable{TEntity}"/> that is not tracked by EF and is safe to mutate.
    /// </summary>
    /// <remarks>
    /// WARNING: Make sure to include your own filtering clauses and pagination to avoid returning all data from the table into memory.
    /// Avoid calling .ToList() unless you really need all the data.
    /// </remarks>
    /// <returns></returns>
    public IQueryable<TEntity> GetEntityQueryable()
    {
        return Context.Set<TEntity>().AsNoTracking().AsQueryable();
    }

    /// <summary>
    /// This returns exactly one entity.  There should be exactly one entity that is found, because we're looking it up by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TEntity> GetSingleEntityByIdAsync(int id)
    {
        return await Context.Set<TEntity>().AsNoTracking().Where(p => p.Id == id).ToAsyncEnumerable().SingleAsync();
    }

    /// <summary>
    /// This returns the first or default value based on the provided where clause
    /// </summary>
    /// <param name="where"></param>
    /// <remarks>
    /// Example use: readonlyRepo.GetFirstOrDefaultEntityByFilterAsync(p => p.AProperty == AValue);
    /// </remarks>
    /// <returns></returns>
    public async Task<TEntity?> GetFirstOrDefaultEntityByFilterAsync(Func<TEntity, bool> where)
    {
        return await Context.Set<TEntity>().AsNoTracking().Where(where).ToAsyncEnumerable().FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets a <see cref="PagedResults{TEntity}"/> of an entity, ordered by Id, which is another way of getting the oldest records first.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <remarks>While entities do have a CreatedDate column, the ID is an auto incremented PK, meaning the bigger the number, the later it was inserted.</remarks>
    /// <returns></returns>
    public Task<PagedResults<TEntity>> GetPaginatedEntityOrderedByIdOldestFirstAsync(int pageNumber, int pageSize)
    {
        return GetPaginatedEntityWithFilterAndOrderAsync(pageNumber, pageSize, o => o.Id);
    }

    /// <summary>
    /// Gets a <see cref="PagedResults{TEntity}"/> of an entity, ordered by Id descending, which is another way of getting the newest records first.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// /// <remarks>While entities do have a CreatedDate column, the ID is an auto incremented PK, meaning the bigger the number, the later it was inserted.</remarks>
    /// <returns></returns>
    public Task<PagedResults<TEntity>> GetPaginatedEntityOrderedByIdNewestFirstAsync(int pageNumber, int pageSize)
    {
        return GetPaginatedEntityWithFilterAndOrderAsync(pageNumber, pageSize, o => o.Id, orderByAscending: false);
    }

    /// <summary>
    /// Gets a <see cref="PagedResults{TEntity}"/> of an entity, ordered by provided expression ascending, and accepts an optional where filter.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="order"></param>
    /// <param name="where"></param>
    /// <param name="includeEntityName"></param>
    /// <remarks>
    /// Example use repo.GetPaginatedEntityAscendingAsync(1, 2, o => o.AProperty, p => p.AProperty == AValue)
    /// <para />
    /// WARNING: If using the include, be aware that all related entities will be fetched, included entities are not paginated.
    /// <para />
    /// Example use of include repo.GetPaginatedEntityAscendingAsync(1, 2, o => o.AProperty, p => p.AProperty == AValue, nameof(RepoEntity.RepoNavigationProperty))
    /// </remarks>
    /// <returns></returns>
    public Task<PagedResults<TEntity>> GetPaginatedEntityAscendingAsync(int pageNumber, int pageSize, Expression<Func<TEntity, object>> order, Func<TEntity, bool>? where = null, string? includeEntityName = null)
    {
        return GetPaginatedEntityWithFilterAndOrderAsync(pageNumber, pageSize, order, orderByAscending: true, where, includeEntityName);
    }

    /// <summary>
    /// Gets a <see cref="PagedResults{TEntity}"/> of an entity, ordered by provided expression descending, and accepts an optional where filter.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="order"></param>
    /// <param name="where"></param>
    /// <param name="includeEntityName"></param>
    /// <remarks>
    /// Example use repo.GetPaginatedEntityDescendingAsync(1, 2, o => o.AProperty, p => p.AProperty == AValue)
    /// <para />
    /// WARNING: If using the include, be aware that all related entities will be fetched, included entities are not paginated.
    /// <para />
    ///  Example use of include repo.GetPaginatedEntityDescendingAsync(1, 2, o => o.AProperty, p => p.AProperty == AValue, nameof(RepoEntity.RepoNavigationProperty))
    /// </remarks>
    /// <returns></returns>
    public Task<PagedResults<TEntity>> GetPaginatedEntityDescendingAsync(int pageNumber, int pageSize, Expression<Func<TEntity, object>> order, Func<TEntity, bool>? where = null, string? includeEntityName = null)
    {
        return GetPaginatedEntityWithFilterAndOrderAsync(pageNumber, pageSize, order, orderByAscending: false, where, includeEntityName);
    }

    private Task<PagedResults<TEntity>> GetPaginatedEntityWithFilterAndOrderAsync(int pageNumber, int pageSize, Expression<Func<TEntity, object>> order, bool orderByAscending = true, Func<TEntity, bool>? where = null, string? includeEntityName = null)
    {
        var query = Context.Set<TEntity>().AsNoTracking().AsQueryable();

        if (where != null)
        {
            query = query.Where(where).AsQueryable();
        }

        if (orderByAscending)
        {
            query = query.OrderBy(order);
        }
        else
        {
            query = query.OrderByDescending(order);
        }

        if (!string.IsNullOrEmpty(includeEntityName))
        {
            return query.Include(includeEntityName).ToPaginatedResultsAsync(pageNumber: pageNumber, pageSize: pageSize);
        }

        return query.ToPaginatedResultsAsync(pageNumber: pageNumber, pageSize: pageSize);
    }
}
