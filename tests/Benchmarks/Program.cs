using BenchmarkDotNet.Running;
using Benchmarks.Data;

BenchmarkSwitcher.FromAssembly(assembly: typeof(BenchmarkDbContext).Assembly).Run(args);
