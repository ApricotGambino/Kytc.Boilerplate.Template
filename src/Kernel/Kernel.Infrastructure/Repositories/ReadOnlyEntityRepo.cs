namespace KernelInfrastructure.Repositories;

using KernelData.Entities;
using KernelData.EntityFramework;
using KernelData.Extensions.Pagination;
using Microsoft.EntityFrameworkCore;


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
    /// WARNING: Make sure to include your own filtering clauses to avoid returning all data from the table into memory.
    /// Avoid calling .ToList() unless you really need all the data.
    /// </remarks>
    /// <returns></returns>
    public IQueryable<TEntity> GetEntityQueryable()
    {
        return Context.Set<TEntity>().AsNoTracking().AsQueryable();
    }

    public Task<PagedResults<TEntity>> GetPaginatedEntityOrderedByIdAsync(int pageNumber, int pageSize)
    {
        var results = Context.Set<TEntity>().AsNoTracking().AsQueryable();
        return results.OrderBy(o => o.Id).ToPaginatedResultsAsync(pageNumber: pageNumber, pageSize: pageSize);
    }

    public Task<PagedResults<TEntity>> TEST(Func<TEntity, bool> where, int pageNumber, int pageSize)
    {
        var results = Context.Set<TEntity>().AsNoTracking().Where(where).AsQueryable();
        return results.ToPaginatedResultsAsync(pageNumber: pageNumber, pageSize: pageSize);
    }
}

//public class ReadOnlyEntityRepo<TEntity>() //: IReadOnlyEntityRepo<TEntity>
//    where TEntity : BaseEntity
//{
//    public IQueryable<TEntity> GetEntityQueryable()
//    {
//        return new List<TEntity>().AsQueryable();
//    }
//}
