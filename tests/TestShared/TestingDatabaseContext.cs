namespace TestShared;

using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TestShared.TestObjects;

public class TestingDatabaseContext : ApplicationDbContext
{
    public TestingDatabaseContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<TestEntity> TestEntities { get; set; }
}
