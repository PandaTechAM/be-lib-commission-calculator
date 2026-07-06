using CommissionCalculator.DTO;

namespace CommissionCalculator.Internal;

internal sealed class NormalizedCommissionRule
{
    internal readonly CalculationType _calcType;
    internal readonly short _decimalPlaces;

    // prefix[i] = sum of full-range commissions for Ranges[0..i-1]
    internal readonly decimal[] _proportionalPrefix;

    // sorted by Start, contiguous, End is exclusive; last End = decimal.MaxValue
    internal readonly NormalizedRange[] _ranges;

    internal NormalizedCommissionRule(CalculationType calcType,
        short decimalPlaces,
        NormalizedRange[] ranges,
        decimal[] proportionalPrefix)
    {
        _calcType = calcType;
        _decimalPlaces = decimalPlaces;
        _ranges = ranges;
        _proportionalPrefix = proportionalPrefix;
    }
}
