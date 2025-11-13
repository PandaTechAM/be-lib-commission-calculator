using System.Runtime.CompilerServices;
using CommissionCalculator.DTO;

namespace CommissionCalculator.Internal;

internal static class CommissionRuleCache
{
   private static readonly ConditionalWeakTable<CommissionRule, NormalizedCommissionRule> Cache = new();

   public static NormalizedCommissionRule Get(CommissionRule rule) => Cache.GetValue(rule, Normalize);

   private static NormalizedCommissionRule Normalize(CommissionRule rule)
   {
      // Reuse public validation contract (throws if invalid)
      if (!Commission.ValidateRule(rule))
      {
         throw new ArgumentException("Invalid commission rule.");
      }

      // Convert once: map 0 => +∞ for RangeEnd/Max, then sort by Start
      var src = rule.CommissionRangeConfigs;
      var list = new List<NormalizedRange>(src.Count);
      foreach (var c in src)
      {
         var end = c.RangeEnd == 0 ? decimal.MaxValue : c.RangeEnd;
         var max = c.MaxCommission == 0 ? decimal.MaxValue : c.MaxCommission;
         list.Add(new NormalizedRange(c.RangeStart, end, c.Type, c.CommissionAmount, c.MinCommission, max));
      }

      list.Sort((a, b) => a.Start.CompareTo(b.Start));
      var ranges = list.ToArray();

      // Precompute proportional prefix (full-range contributions). Last +∞ contributes 0 (never fully consumed).
      decimal[] prefix;
      if (rule.CalculationType == CalculationType.Proportional)
      {
         prefix = new decimal[ranges.Length + 1];
         for (var i = 0; i < ranges.Length; i++)
         {
            var r = ranges[i];
            if (r.End == decimal.MaxValue)
            {
               prefix[i + 1] = prefix[i];
               continue;
            }

            var width = r.End - r.Start; // finite
            prefix[i + 1] = prefix[i] + FastPath.ComputeRangeCommission(r.Type, r.Amount, r.Min, r.Max, width);
         }
      }
      else
      {
         prefix = [];
      }

      return new NormalizedCommissionRule(rule.CalculationType, rule.DecimalPlace, ranges, prefix);
   }
}