
using System.Collections;
using BenchmarkDotNet.Attributes;
using Benchmarks.Data;
using Microsoft.EntityFrameworkCore;

namespace Benchmarks.Tests;
//Results as of 2/18/2026:
// * Summary *

//BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.6899/25H2/2025Update/HudsonValley2)
//AMD Ryzen 5 7600 3.80GHz, 1 CPU, 12 logical and 6 physical cores
//.NET SDK 10.0.102
//  [Host]   : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v4
//  ShortRun : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v4

//Job = ShortRun  IterationCount=3  LaunchCount=1
//WarmupCount=3

//| Method                          | numberOfRecordsToTake | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0    | Code Size | Completed Work Items | Lock Contentions | Gen1    | Allocated  | Alloc Ratio |
//|-------------------------------- |---------------------- |-----------:|-----------:|----------:|------:|--------:|--------:|----------:|---------------------:|-----------------:|--------:|-----------:|------------:|
//| EFCore_NoTake                   | 0                     | 2,060.8 us |   132.6 us |   7.27 us |  1.00 |    0.00 | 97.6563 |     246 B |              43.0000 |                - | 39.0625 | 1664.37 KB |        1.00 |
//| EFCore_WithTake                 | 0                     |   172.5 us |   577.1 us |  31.63 us |  0.08 |    0.01 |  5.3711 |   7,757 B |               3.0000 |                - |  0.4883 |   92.67 KB |        0.06 |
//| EFCore_WithTake_AndAsNoTracking | 0                     |   187.9 us |   647.0 us |  35.46 us |  0.09 |    0.01 |  5.3711 |   7,778 B |               3.0000 |                - |  0.4883 |   93.76 KB |        0.06 |
//|                                 |                       |            |            |           |       |         |         |           |                      |                  |         |            |             |
//| EFCore_NoTake                   | 1                     | 2,098.9 us |   380.8 us |  20.87 us |  1.00 |    0.01 | 85.9375 |     246 B |              43.0000 |                - | 35.1563 | 1664.32 KB |        1.00 |
//| EFCore_WithTake                 | 1                     |   186.1 us |   829.2 us |  45.45 us |  0.09 |    0.02 |  5.3711 |   7,391 B |               3.0000 |                - |  0.4883 |   94.52 KB |        0.06 |
//| EFCore_WithTake_AndAsNoTracking | 1                     |   193.3 us |   790.1 us |  43.31 us |  0.09 |    0.02 |  5.3711 |   7,784 B |               3.0005 |                - |  0.4883 |   94.52 KB |        0.06 |
//|                                 |                       |            |            |           |       |         |         |           |                      |                  |         |            |             |
//| EFCore_NoTake                   | 10                    | 2,046.6 us |   423.8 us |  23.23 us |  1.00 |    0.01 | 97.6563 |     246 B |              43.0000 |                - | 39.0625 | 1664.37 KB |        1.00 |
//| EFCore_WithTake                 | 10                    |   227.5 us |   339.8 us |  18.63 us |  0.11 |    0.01 |  6.3477 |   7,353 B |               3.0000 |                - |  0.4883 |  106.49 KB |        0.06 |
//| EFCore_WithTake_AndAsNoTracking | 10                    |   331.9 us |   717.0 us |  39.30 us |  0.16 |    0.02 |  5.8594 |     314 B |               3.0000 |                - |       - |  101.33 KB |        0.06 |
//|                                 |                       |            |            |           |       |         |         |           |                      |                  |         |            |             |
//| EFCore_NoTake                   | 20                    | 2,391.7 us |   646.0 us |  35.41 us |  1.00 |    0.02 | 97.6563 |     246 B |              43.0000 |                - | 39.0625 | 1664.37 KB |        1.00 |
//| EFCore_WithTake                 | 20                    |   347.2 us |   375.3 us |  20.57 us |  0.15 |    0.01 |  6.8359 |     314 B |               3.0000 |           0.0010 |  0.9766 |  121.39 KB |        0.07 |
//| EFCore_WithTake_AndAsNoTracking | 20                    |   317.3 us |   350.7 us |  19.22 us |  0.13 |    0.01 |  5.8594 |     314 B |               3.0000 |           0.0010 |       - |  107.72 KB |        0.06 |
//|                                 |                       |            |            |           |       |         |         |           |                      |                  |         |            |             |
//| EFCore_NoTake                   | 50                    | 2,198.3 us |   875.3 us |  47.98 us |  1.00 |    0.03 | 85.9375 |     246 B |              43.0000 |                - | 35.1563 | 1664.32 KB |        1.00 |
//| EFCore_WithTake                 | 50                    |   400.3 us |   287.3 us |  15.75 us |  0.18 |    0.01 |  9.7656 |     314 B |               3.0010 |                - |  1.9531 |  161.32 KB |        0.10 |
//| EFCore_WithTake_AndAsNoTracking | 50                    |   374.5 us |   259.2 us |  14.21 us |  0.17 |    0.01 |  7.8125 |     314 B |               3.0000 |                - |  0.9766 |  127.63 KB |        0.08 |
//|                                 |                       |            |            |           |       |         |         |           |                      |                  |         |            |             |
//| EFCore_NoTake                   | 100                   | 2,365.4 us | 2,649.9 us | 145.25 us |  1.00 |    0.08 | 89.8438 |     246 B |              42.9961 |                - | 35.1563 | 1664.33 KB |        1.00 |
//| EFCore_WithTake                 | 100                   |   532.0 us |   440.3 us |  24.13 us |  0.23 |    0.02 | 13.6719 |     246 B |               5.0000 |                - |  1.9531 |  240.46 KB |        0.14 |
//| EFCore_WithTake_AndAsNoTracking | 100                   |   401.6 us |   542.6 us |  29.74 us |  0.17 |    0.01 |  9.7656 |     314 B |               5.0010 |                - |  1.9531 |  169.59 KB |        0.10 |
//|                                 |                       |            |            |           |       |         |         |           |                      |                  |         |            |             |
//| EFCore_NoTake                   | 1000                  | 2,184.0 us | 1,927.2 us | 105.63 us |  1.00 |    0.06 | 93.7500 |     246 B |              43.0000 |           0.0039 | 39.0625 | 1664.35 KB |        1.00 |
//| EFCore_WithTake                 | 1000                  | 2,325.4 us | 2,577.2 us | 141.26 us |  1.07 |    0.07 | 93.7500 |     246 B |              42.9961 |                - | 39.0625 | 1667.48 KB |        1.00 |
//| EFCore_WithTake_AndAsNoTracking | 1000                  | 1,548.6 us | 1,322.6 us |  72.49 us |  0.71 |    0.04 | 58.5938 |     314 B |              43.0000 |           0.0020 | 23.4375 |  960.61 KB |        0.58 |

//// * Legends *
//  numberOfRecordsToTake : Value of the 'numberOfRecordsToTake' parameter
//  Mean                  : Arithmetic mean of all measurements
//  Error                 : Half of 99.9% confidence interval
//  StdDev                : Standard deviation of all measurements
//  Ratio                 : Mean of the ratio distribution([Current]/[Baseline])
//  RatioSD               : Standard deviation of the ratio distribution([Current]/[Baseline])
//  Gen0                  : GC Generation 0 collects per 1000 operations
//  Code Size             : Native code size of the disassembled method(s)
//  Completed Work Items  : The number of work items that have been processed in ThreadPool(per single operation)
//  Lock Contentions      : The number of times there was contention upon trying to take a Monitor's lock (per single operation)
//  Gen1                  : GC Generation 1 collects per 1000 operations
//  Allocated             : Allocated memory per single operation(managed only, inclusive, 1KB = 1024B)
//  Alloc Ratio           : Allocated memory ratio distribution([Current]/[Baseline])
//  1 us                  : 1 Microsecond(0.000001 sec)


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
        var results = await context.TestEntities.ToListAsync();
        return results.Count;
    }

    [Benchmark]
    [TestCaseSource(typeof(NumberOfRecordsToTakeTestCase), nameof(NumberOfRecordsToTakeTestCase.TestCases))]
    [ArgumentsSource(nameof(NumberOfRecordsToTakeBenchmark))]
    public async Task<int> EFCore_WithTake(int numberOfRecordsToTake)
    {
        await using var context = new BenchmarkDbContext();

        var results = await context.TestEntities.Take(numberOfRecordsToTake).ToListAsync();
        return results.Count;
    }

    [Benchmark]
    [TestCaseSource(typeof(NumberOfRecordsToTakeTestCase), nameof(NumberOfRecordsToTakeTestCase.TestCases))]
    [ArgumentsSource(nameof(NumberOfRecordsToTakeBenchmark))]
    public async Task<int> EFCore_WithTake_AndAsNoTracking(int numberOfRecordsToTake)
    {
        await using var context = new BenchmarkDbContext();

        var results = await context.TestEntities.AsNoTracking().Take(numberOfRecordsToTake).ToListAsync();
        return results.Count;
    }

    #endregion Tests
}
