namespace Data.EntityFramework;

using KernelData.EntityFramework;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : BaseDbContext(options)
{
    //public DbSet<ExampleEntity> ExampleEntities => Set<ExampleEntity>();
}
