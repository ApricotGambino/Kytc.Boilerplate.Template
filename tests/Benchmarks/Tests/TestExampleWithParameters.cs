
using System.Collections;
using BenchmarkDotNet.Attributes;
using Benchmarks.Data;

namespace Benchmarks.Tests;
//NOTE: There's a guide on how to use benchmarking in \assets\Assets\root\docfx\articles\guides\benchmark-testing.md

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[ThreadingDiagnoser]
[ShortRunJob]
[Category(TestingCategoryConstants.BenchmarkTests)]
public class TestExampleWithParameters
{
    #region Setup
    /// <summary>
    /// This method setups the test and benchmark. Using nUnit's OneTimeSetup and DotNetBenchmark's GlobalSetup
    /// </summary>
    [GlobalSetup]
    [OneTimeSetUp]
    public static Task SetupAsync()
    {
        //Here you can do some setup, likely seed some data.
        return BenchmarkDbContext.SeedTestEntitiesAsync(1000);
    }

    /// <summary>
    /// This is the nUnit test case source, it needs to mimic the paramaterized test for DotNetBenchmark,
    /// and will be executed with expected results in the test explorer, this way you know your benchmark
    /// is at least working as expected when you get your benchmark results.
    /// </summary>
    public static class NUnitTestCaseSource
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(0).Returns(0);
                yield return new TestCaseData(1000).Returns(1000);
            }
        }
    }
    /// <summary>
    /// This is the Argument Source for DotNetBenchmark, these values will be passed into
    /// each benchmark, we make sure the nUnit test case source matches this so
    /// we know the results do what we expect, then use this argument source to benchmark against.
    /// </summary>
    public static int[] DotNetBenchMarkArgumentSource() { return [0, 1, 10, 20, 50, 100, 1000]; }

    #endregion Setup

    #region Tests
    [Benchmark(Baseline = true)]
    [TestCaseSource(typeof(NUnitTestCaseSource), nameof(NUnitTestCaseSource.TestCases))]
    [ArgumentsSource(nameof(DotNetBenchMarkArgumentSource))]
    public async Task<int> Example_ThingToTest(int parameterUsedInTest)
    {
        //This method is defined as the baseline, which means all other tests in this suite
        //will be compared against it.

        //Do some action using parameterUsedInTest

        //Return some value to be tested against from the
        return parameterUsedInTest;
    }

    [Benchmark]
    [TestCaseSource(typeof(NUnitTestCaseSource), nameof(NUnitTestCaseSource.TestCases))]
    [ArgumentsSource(nameof(DotNetBenchMarkArgumentSource))]
    public async Task<int> Example_AnotherThingToTest(int parameterUsedInTest)
    {
        //Do some action using parameterUsedInTest

        //Return some value to be tested against from the
        return parameterUsedInTest;
    }
    #endregion Tests
}
