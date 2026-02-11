namespace Benchmarks.Tests;

using System.Collections;
using BenchmarkDotNet.Attributes;
using Benchmarks.Data;
using Microsoft.EntityFrameworkCore;


[MemoryDiagnoser]
[DisassemblyDiagnoser]
[ThreadingDiagnoser]
[ShortRunJob]
[Category(TestingCategoryConstants.BenchmarkTests)]
public class EFCoreToListBenchmarks
{
    #region Setup

    /// <summary>
    /// This method setups the test and benchmark. Using nUnit's OneTimeSetup and DotNetBenchmark's GlobalSetup
    /// </summary>
    /// <returns></returns>
    [GlobalSetup]
    [OneTimeSetUp]
    public static Task Setup()
    {
        return BenchmarkDbContext.SeedTestEntitiesAsync(1000);
    }

    /// <summary>
    /// This is the nUnit test case source, it needs to mimic the paramaterized test for DotNetBenchmark,
    /// and will be executed with expected results in the test explorer, this way you know your benchmark
    /// is at least working as expected when you get your benchmark results.
    /// </summary>
    public static class NumberOfRecordsToTakeTestCase
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(0).Returns(0);
                yield return new TestCaseData(1).Returns(1);
                yield return new TestCaseData(10).Returns(10);
                yield return new TestCaseData(20).Returns(20);
                yield return new TestCaseData(50).Returns(50);
                yield return new TestCaseData(100).Returns(100);
                yield return new TestCaseData(1000).Returns(1000);
            }
        }
    }
    /// <summary>
    /// This is the Argument Source for DotNetBenchmark, these values will be passed into
    /// each benchmark, we make sure the nUnit test case source matches this so
    /// we know the results do what we expect, then use this argument source to benchmark against.
    /// </summary>
    public static int[] NumberOfRecordsToTakeBenchmark() { return [0, 1, 10, 20, 50, 100, 1000]; }

    #endregion Setup

    #region Tests
    [Benchmark(Baseline = true)]
    [TestCase(0, ExpectedResult = 1000)]
    [ArgumentsSource(nameof(NumberOfRecordsToTakeBenchmark))]
    public async Task<int> EFCore_NoTake(int numberOfRecordsToTake)
    {
        //NOTE: even though we're not using the numberOfRecordsToTake,
        //because we're getting all of them every time, in order to make this
        //the baseline benchmark in which we compare all results we have to follow the same
        //benchmark structure of all other tests.
        _ = numberOfRecordsToTake;
        await using var context = new BenchmarkDbContext();
        var results = await context.TestObjects.ToListAsync();
        return results.Count;
    }

    [Benchmark]
    [TestCaseSource(typeof(NumberOfRecordsToTakeTestCase), nameof(NumberOfRecordsToTakeTestCase.TestCases))]
    [ArgumentsSource(nameof(NumberOfRecordsToTakeBenchmark))]
    public async Task<int> EFCore_WithTake(int numberOfRecordsToTake)
    {
        await using var context = new BenchmarkDbContext();

        var results = await context.TestObjects.Take(numberOfRecordsToTake).ToListAsync();
        return results.Count;
    }

    [Benchmark]
    [TestCaseSource(typeof(NumberOfRecordsToTakeTestCase), nameof(NumberOfRecordsToTakeTestCase.TestCases))]
    [ArgumentsSource(nameof(NumberOfRecordsToTakeBenchmark))]
    public async Task<int> EFCore_WithTake_AndAsNoTracking(int numberOfRecordsToTake)
    {
        await using var context = new BenchmarkDbContext();

        var results = await context.TestObjects.AsNoTracking().Take(numberOfRecordsToTake).ToListAsync();
        return results.Count;
    }

    #endregion Tests
}
