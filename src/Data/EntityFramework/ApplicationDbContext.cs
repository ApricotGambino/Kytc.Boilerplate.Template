namespace Data.EntityFramework;

using Data.Entities.Example;
using KernelData.EntityFramework;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : BaseDbContext(options)
{
    public DbSet<ExampleEntity> ExampleEntities => Set<ExampleEntity>();
}
