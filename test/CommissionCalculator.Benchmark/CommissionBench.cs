using BenchmarkDotNet.Attributes;
using CommissionCalculator.DTO;

namespace CommissionCalculator.Benchmark;

[MemoryDiagnoser]
public class UnifiedBench
{
    private CommissionRule _absRule = null!;

    private CommissionRule _propRule = null!;

    // keep the param space tiny (fast + comparable rows)
    public static IEnumerable<decimal> Principals => [2_000m]; // fixed
    public static IEnumerable<decimal> Selectors => [1.5m, 3m, 5m]; // one per selector range

    [ParamsSource(nameof(Principals))]
    public decimal Principal { get; set; }

    [ParamsSource(nameof(Selectors))]
    public decimal Selector { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _propRule = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs =
            [
                new CommissionRangeConfigs
                {
                    RangeStart = 0,
                    RangeEnd = 500,
                    Type = CommissionType.FlatRate,
                    CommissionAmount = 25,
                    MinCommission = 0,
                    MaxCommission = 0
                },
                new CommissionRangeConfigs
                {
                    RangeStart = 500,
                    RangeEnd = 1000,
                    Type = CommissionType.Percentage,
                    CommissionAmount = 0.1m,
                    MinCommission = 0,
                    MaxCommission = 0
                },
                new CommissionRangeConfigs
                {
                    RangeStart = 1000,
                    RangeEnd = 10000,
                    Type = CommissionType.Percentage,
                    CommissionAmount = 0.2m,
                    MinCommission = 250,
                    MaxCommission = 1500
                },
                new CommissionRangeConfigs
                {
                    RangeStart = 10000,
                    RangeEnd = 0,
                    Type = CommissionType.FlatRate,
                    CommissionAmount = 2000,
                    MinCommission = 0,
                    MaxCommission = 0
                }
            ]
        };

        _absRule = new CommissionRule
        {
            CalculationType = CalculationType.Absolute,
            DecimalPlace = 0,
            CommissionRangeConfigs =
            [
                new CommissionRangeConfigs
                {
                    RangeStart = 0,
                    RangeEnd = 2,
                    Type = CommissionType.FlatRate,
                    CommissionAmount = 50,
                    MinCommission = 0,
                    MaxCommission = 0
                },
                new CommissionRangeConfigs
                {
                    RangeStart = 2,
                    RangeEnd = 4,
                    Type = CommissionType.Percentage,
                    CommissionAmount = 0.10m,
                    MinCommission = 0,
                    MaxCommission = 0
                },
                new CommissionRangeConfigs
                {
                    RangeStart = 4,
                    RangeEnd = 0,
                    Type = CommissionType.FlatRate,
                    CommissionAmount = 100,
                    MinCommission = 0,
                    MaxCommission = 0
                }
            ]
        };
    }

    [Benchmark(Baseline = true)]
    public decimal Proportional_PrincipalBased()
    {
        return Commission.ComputeCommission(Principal, _propRule);
    }

    [Benchmark]
    public decimal Absolute_PrincipalBased()
    {
        return Commission.ComputeCommission(Principal, _absRule);
    }

    [Benchmark]
    public decimal Absolute_SelectorBased()
    {
        return Commission.ComputeCommission(Principal, Selector, _absRule);
    }
}
