using BenchmarkDotNet.Attributes;
using CommissionCalculator.DTO;

namespace CommissionCalculator.Benchmark;

[MemoryDiagnoser]
public class CommissionBench
{
   private CommissionRule _proportionalRule = null!;
   private CommissionRule _absoluteRule = null!;
   private decimal _principal;
   private decimal _selector;

   // Sources for decimal params
   public static IEnumerable<decimal> PrincipalValues => [755_789m, 1_000m, 25_000m];
   public static IEnumerable<decimal> SelectorValues => [1m, 3m, 5m, 10m];

   [ParamsSource(nameof(PrincipalValues))]
   public decimal PrincipalParam { get; set; }

   [ParamsSource(nameof(SelectorValues))]
   public decimal SelectorParam { get; set; }

   [GlobalSetup]
   public void Setup()
   {
      _principal = PrincipalParam;
      _selector = SelectorParam;

      _proportionalRule = new CommissionRule
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

      _absoluteRule = new CommissionRule
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
   public decimal Proportional_PrincipalBased() => Commission.ComputeCommission(_principal, _proportionalRule);

   [Benchmark]
   public decimal Absolute_PrincipalBased() => Commission.ComputeCommission(_principal, _absoluteRule);

   [Benchmark]
   public decimal Absolute_SelectorBased() => Commission.ComputeCommission(_principal, _selector, _absoluteRule);
}