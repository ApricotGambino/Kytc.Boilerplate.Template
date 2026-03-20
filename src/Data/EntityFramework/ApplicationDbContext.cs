
using Data.Entities.ADifferentExampleSchema;
using Data.Entities.ExampleSchema;
using Kernel.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Data.EntityFramework;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : BaseDbContext(options)
{
    public DbSet<ExampleEntity> ExampleEntities => Set<ExampleEntity>();

    public DbSet<ADifferentExampleEntity> ADifferentExampleEntites => Set<ADifferentExampleEntity>();
}
