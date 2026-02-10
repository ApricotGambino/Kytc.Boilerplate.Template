namespace Infrastructure.Repositories;

using Data.EntityFramework;
using Data.Kernel.Entities;
using Microsoft.EntityFrameworkCore;

public class ReadOnlyEntityRepo<TEntity>(ApplicationDbContext context)
    where TEntity : BaseEntity
{
    protected ApplicationDbContext Context { get; set; } = context;
    public IQueryable<TEntity> GetEntityQueryable()
    {
        return Context.Set<TEntity>().AsNoTracking().AsQueryable();
    }
}
