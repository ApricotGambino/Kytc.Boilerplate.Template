namespace KernelInfrastructure.Repositories;

using KernelData.Entities;
using KernelData.EntityFramework;
using Microsoft.EntityFrameworkCore;



public class ReadOnlyEntityRepo<TEntity, TDatabaseContext>(TDatabaseContext context) //: IReadOnlyEntityRepo<TEntity>
    where TEntity : BaseEntity
    where TDatabaseContext : BaseDbContext
{
    protected TDatabaseContext Context { get; set; } = context;
    public IQueryable<TEntity> GetEntityQueryable()
    {
        return Context.Set<TEntity>().AsNoTracking().AsQueryable();
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
