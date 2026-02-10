namespace TestShared;

using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using TestShared.TestObjects;

public class TestingDatabaseContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
{
    /// <summary>
    /// These entities are used only in the Testing Database Context, this entity should not be added to the ApplicationDBContext.
    /// </summary>
    public DbSet<TestEntity> TestEntities { get; set; }
}
