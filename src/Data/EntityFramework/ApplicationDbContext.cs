
using Data.Entities.Example;
using KernelData.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Data.EntityFramework;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : BaseDbContext(options)
{
    public DbSet<ExampleEntity> ExampleEntities => Set<ExampleEntity>();
}
