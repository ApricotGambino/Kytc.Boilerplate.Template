namespace Benchmarks.Tests;

using System.Collections;
using BenchmarkDotNet.Attributes;
using Benchmarks.Data;
using Microsoft.EntityFrameworkCore;
using TestShared.TestObjects;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[ThreadingDiagnoser]
[ShortRunJob]
[Category(TestingCategoryConstants.BenchmarkTests)]
public class EFCore_Add
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
        return BenchmarkDbContext.TearDownAndSetupDatabaseAsync();
    }

    /// <summary>
    /// This is the nUnit test case source, it needs to mimic the paramaterized test for DotNetBenchmark,
    /// and will be executed with expected results in the test explorer, this way you know your benchmark
    /// is at least working as expected when you get your benchmark results.
    /// </summary>
    public static class NumberOfRecordsToCreateTestCase
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
    public static int[] NumberOfRecordsToCreateBenchmark() { return [0, 1, 10, 20, 50, 100, 1000]; }

    #endregion Setup

    #region Tests
    [Benchmark(Baseline = true)]
    [TestCaseSource(typeof(NumberOfRecordsToCreateTestCase), nameof(NumberOfRecordsToCreateTestCase.TestCases))]
    [ArgumentsSource(nameof(NumberOfRecordsToCreateBenchmark))]
    public async Task<int> EFCore_AddInLoop(int numberOfRecordsToCreate)
    {
        await using var context = new BenchmarkDbContext();
        var entities = TestEntityHelper.CreateTestEntityList(numberOfRecordsToCreate);

        foreach (var entity in entities)
        {
            context.Add(entity);
        }

        return await context.SaveChangesAsync();
    }

    [Benchmark]
    [TestCaseSource(typeof(NumberOfRecordsToCreateTestCase), nameof(NumberOfRecordsToCreateTestCase.TestCases))]
    [ArgumentsSource(nameof(NumberOfRecordsToCreateBenchmark))]
    public async Task<int> EFCore_AddRange(int numberOfRecordsToCreate)
    {
        await using var context = new BenchmarkDbContext();
        var entities = TestEntityHelper.CreateTestEntityList(numberOfRecordsToCreate);
        context.AddRange(entities);
        return await context.SaveChangesAsync();
    }

    [Benchmark]
    [TestCaseSource(typeof(NumberOfRecordsToCreateTestCase), nameof(NumberOfRecordsToCreateTestCase.TestCases))]
    [ArgumentsSource(nameof(NumberOfRecordsToCreateBenchmark))]
    public async Task<int> EFCore_AddInLoopAsync(int numberOfRecordsToCreate)
    {
        await using var context = new BenchmarkDbContext();
        var entities = TestEntityHelper.CreateTestEntityList(numberOfRecordsToCreate);

        foreach (var entity in entities)
        {
            await context.AddAsync(entity);
        }

        return await context.SaveChangesAsync();
    }

    [Benchmark]
    [TestCaseSource(typeof(NumberOfRecordsToCreateTestCase), nameof(NumberOfRecordsToCreateTestCase.TestCases))]
    [ArgumentsSource(nameof(NumberOfRecordsToCreateBenchmark))]
    public async Task<int> EFCore_AddRangeAsync(int numberOfRecordsToCreate)
    {
        await using var context = new BenchmarkDbContext();
        var entities = TestEntityHelper.CreateTestEntityList(numberOfRecordsToCreate);
        await context.AddRangeAsync(entities);
        return await context.SaveChangesAsync();
    }

    #endregion Tests
}
