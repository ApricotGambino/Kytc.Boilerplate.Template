// BaseDbContext.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning


using Kernel.Data.Entities.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Kernel.Data.EntityFramework;

public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }
    public DbSet<Log> Logs => Set<Log>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyBaseEntityConfiguration();
    }
}

