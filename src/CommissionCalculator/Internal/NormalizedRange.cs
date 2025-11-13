using CommissionCalculator.DTO;

namespace CommissionCalculator.Internal;

internal readonly struct NormalizedRange(
   decimal start,
   decimal end,
   CommissionType type,
   decimal amount,
   decimal min,
   decimal max)
{
   public decimal Start { get; } = start;
   public decimal End { get; } = end; // decimal.MaxValue => +∞
   public CommissionType Type { get; } = type;
   public decimal Amount { get; } = amount; // percentage or flat
   public decimal Min { get; } = min;
   public decimal Max { get; } = max; // decimal.MaxValue => +∞
}