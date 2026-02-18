namespace Benchmarks.Data;

using Microsoft.EntityFrameworkCore;
using TestShared.TestObjects;

public class BenchmarkDbContext : DbContext
{
    public DbSet<TestEntity> TestEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database={TestingConstants.BenchmarkTestsDatabaseName};ConnectRetryCount=0");

    public static async Task SeedTestEntitiesAsync(int numberOfTestObjectsToCreate)
    {
        await using var context = new BenchmarkDbContext();
        await TearDownAndSetupDatabaseAsync(context);
        await InsertTestEntitiesAsync(context, numberOfTestObjectsToCreate);
    }

    public static async Task TearDownAndSetupDatabaseAsync()
    {
        await using var context = new BenchmarkDbContext();

        await TearDownAndSetupDatabaseAsync(context);
    }
    public static async Task TearDownAndSetupDatabaseAsync(BenchmarkDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public static async Task<int> InsertTestEntitiesAsync(int numberOfTestObjectsToCreate)
    {
        await using var context = new BenchmarkDbContext();

        return await InsertTestEntitiesAsync(context, numberOfTestObjectsToCreate);
    }
    public static async Task<int> InsertTestEntitiesAsync(BenchmarkDbContext context, int numberOfTestObjectsToCreate)
    {
        var testObjectsToInsert = TestEntityHelper.CreateTestEntityList(numberOfTestObjectsToCreate);

        await context.TestEntities.AddRangeAsync(testObjectsToInsert);
        return await context.SaveChangesAsync();
    }
}
