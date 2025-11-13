using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CommissionCalculator.Benchmark;

var job = Job.Default
             .WithRuntime(CoreRuntime.Core90)
             .WithLaunchCount(1)
             .WithWarmupCount(1)
             .WithIterationCount(6);

var config = DefaultConfig.Instance.AddJob(job);
BenchmarkRunner.Run<UnifiedBench>(config);