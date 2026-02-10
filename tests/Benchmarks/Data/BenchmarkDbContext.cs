//namespace Benchmarks.Data;

//using Microsoft.EntityFrameworkCore;
//using TestShared.TestObjects;

//public class BenchmarkDbContext : DbContext
//{
//    public DbSet<TestEntity> TestObjects { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template.Benchmark;ConnectRetryCount=0");

//    public static async Task SeedAsync(int numberOfTestObjectsToCreate)
//    {

//        using var context = new BenchmarkDbContext();
//        await context.Database.EnsureDeletedAsync();
//        await context.Database.EnsureCreatedAsync();


//        var testObjectsToInsert = TestObjectUsingBaseEntityHelper.CreateTestObjectList(numberOfTestObjectsToCreate);

//        await context.TestObjects.AddRangeAsync(testObjectsToInsert);
//        await context.SaveChangesAsync();
//    }
//}
