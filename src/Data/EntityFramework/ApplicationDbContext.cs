
using Data.Entities.Example;
using Kernel.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Data.EntityFramework;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : BaseDbContext(options)
{
    public DbSet<ExampleEntity> ExampleEntities => Set<ExampleEntity>();
}
