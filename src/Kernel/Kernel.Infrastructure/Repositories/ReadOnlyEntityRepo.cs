namespace KernelInfrastructure.Repositories;

using KernelData.Entities;
using Microsoft.EntityFrameworkCore;

public class ReadOnlyEntityRepo<TEntity>(DbContext context)
    where TEntity : BaseEntity
{
    protected DbContext Context { get; set; } = context;
    public IQueryable<TEntity> GetEntityQueryable()
    {
        return Context.Set<TEntity>().AsNoTracking().AsQueryable();
    }
}
