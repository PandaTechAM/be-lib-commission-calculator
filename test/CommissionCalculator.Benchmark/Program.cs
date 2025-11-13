using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CommissionCalculator.Benchmark;

var job = Job.Default
             .WithRuntime(CoreRuntime.Core90)
             .WithWarmupCount(3)
             .WithIterationCount(10)
             .WithLaunchCount(1);

var config = DefaultConfig.Instance.AddJob(job);
BenchmarkRunner.Run<CommissionBench>(config);