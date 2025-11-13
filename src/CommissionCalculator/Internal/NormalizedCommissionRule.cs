using CommissionCalculator.DTO;

namespace CommissionCalculator.Internal;

internal sealed class NormalizedCommissionRule
{
   internal readonly CalculationType CalcType;
   internal readonly short DecimalPlaces;

   // sorted by Start, contiguous, End is exclusive; last End = decimal.MaxValue
   internal readonly NormalizedRange[] Ranges;

   // prefix[i] = sum of full-range commissions for Ranges[0..i-1]
   internal readonly decimal[] ProportionalPrefix;

   internal NormalizedCommissionRule(CalculationType calcType,
      short decimalPlaces,
      NormalizedRange[] ranges,
      decimal[] proportionalPrefix)
   {
      CalcType = calcType;
      DecimalPlaces = decimalPlaces;
      Ranges = ranges;
      ProportionalPrefix = proportionalPrefix;
   }
}