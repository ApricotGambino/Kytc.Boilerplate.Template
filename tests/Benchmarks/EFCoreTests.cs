//namespace Benchmarks;

//using BenchmarkDotNet.Attributes;
//using Benchmarks.Data;
//using Microsoft.EntityFrameworkCore;
//using TestShared.TestObjects;

//[MemoryDiagnoser]
//[DisassemblyDiagnoser]
//[ThreadingDiagnoser]
//[ShortRunJob]

////TODO: Explain how to run these tests, you need to be in this directory and run 'dotnet run -c Release'
//public class EFCore_TakeVsNoTake
//{

//    [GlobalSetup]
//    public Task Setup()
//    {
//        return BenchmarkDbContext.SeedAsync(1000);
//    }

//    [Params(0, 1, 10, 20, 50, 100, 1000)]
//    public int NumberOfRecordsToTake { get; set; }

//    [Benchmark(Baseline = true)]
//    public async Task<List<TestEntity>> EFCore_NoTake()
//    {
//        using var context = new BenchmarkDbContext();

//        return await context.TestObjects.ToListAsync();
//    }

//    [Benchmark]
//    public async Task<List<TestEntity>> EFCore_WithTake()
//    {
//        using var context = new BenchmarkDbContext();

//        return await context.TestObjects.Take(NumberOfRecordsToTake).ToListAsync();
//    }

//    [Benchmark]
//    public async Task<List<TestEntity>> EFCore_WithTake_AndAsNoTracking()
//    {
//        using var context = new BenchmarkDbContext();

//        return await context.TestObjects.AsNoTracking().Take(NumberOfRecordsToTake).ToListAsync();
//    }
//}

////public class Program
////{
////    public static void Main(string[] args)
////    {
////        var summary = BenchmarkRunner.Run<Md5VsSha256>();
////    }
////}
