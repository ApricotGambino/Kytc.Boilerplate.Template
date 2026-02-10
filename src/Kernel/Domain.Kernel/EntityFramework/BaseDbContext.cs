namespace Kernel.Data.EntityFramework;

using Kernel.Data.Entities.Kernel;
using Microsoft.EntityFrameworkCore;

public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }
    public DbSet<Log> Logs => Set<Log>();


    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);
    //    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    //}
}
