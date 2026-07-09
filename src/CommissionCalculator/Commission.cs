using CommissionCalculator.DTO;
using CommissionCalculator.Internal;
using static CommissionCalculator.Internal.FastPath;

namespace CommissionCalculator;

/// <summary>
///     Computes tiered commissions using absolute or proportional models, with binary-search range lookup and cached rule
///     normalization.
/// </summary>
public static class Commission
{
    /// <summary>
    ///     Compute the commission for a principal amount using the tier that contains it.
    /// </summary>
    public static decimal ComputeCommission(decimal principalAmount, CommissionRule rule)
    {
        var nr = CommissionRuleCache.Get(rule);

        var commission = nr._calcType == CalculationType.Proportional
            ? CalculateProportional(principalAmount, nr)
            : CalculateAbsolute(principalAmount, nr);

        return RoundCommission(commission, nr._decimalPlaces);
    }

    /// <summary>
    ///     Compute an absolute commission where the selector value chooses the range and the commission is applied to the
    ///     principal amount. Throws for Proportional rules.
    /// </summary>
    public static decimal ComputeCommission(decimal principalAmount, decimal selectorValue, CommissionRule rule)
    {
        var nr = CommissionRuleCache.Get(rule);
        if (nr._calcType == CalculationType.Proportional)
        {
            throw new InvalidOperationException(
                "Selector-based overload is incompatible with Proportional rules. Use Absolute.");
        }

        var idx = FindRangeIndex(nr, selectorValue);
        var r = nr._ranges[idx];

        var commission = ComputeRangeCommission(r.Type, r.Amount, r.Min, r.Max, principalAmount);
        return RoundCommission(commission, nr._decimalPlaces);
    }

    // ===== Fast paths using normalized rules =====

    private static decimal CalculateAbsolute(decimal principalAmount, NormalizedCommissionRule nr)
    {
        var idx = FindRangeIndex(nr, principalAmount);
        var r = nr._ranges[idx];
        return ComputeRangeCommission(r.Type, r.Amount, r.Min, r.Max, principalAmount);
    }

    private static decimal CalculateProportional(decimal principalAmount, NormalizedCommissionRule nr)
    {
        // Find the current tier
        var idx = FindRangeIndex(nr, principalAmount);
        var r = nr._ranges[idx];

        // Sum of fully completed prior tiers
        var sum = nr._proportionalPrefix.Length == 0 ? 0 : nr._proportionalPrefix[idx];

        // Partial of the current tier
        var portion = principalAmount - r.Start;
        sum += ComputeRangeCommission(r.Type, r.Amount, r.Min, r.Max, portion);

        return sum;
    }

    private static decimal RoundCommission(decimal commission, short decimalPlaces)
    {
        if (decimalPlaces >= 0)
        {
            return Math.Round(commission, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        var factor = Pow10(Math.Abs(decimalPlaces));

        return Math.Round(commission / factor, 0, MidpointRounding.AwayFromZero) * factor;
    }

    private static decimal Pow10(int power)
    {
        var result = 1M;

        for (var i = 0; i < power; i++)
        {
            result *= 10M;
        }

        return result;
    }

    // ===== Validation (public contract) =====

    /// <summary>
    ///     Validate a commission rule; returns false instead of throwing when the rule is malformed.
    /// </summary>
    public static bool ValidateRule(CommissionRule rule)
    {
        try
        {
            ValidateCommissionRule(rule);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static void ValidateCommissionRule(CommissionRule rule)
    {
        if (rule == null || rule.CommissionRangeConfigs.Count == 0)
        {
            throw new ArgumentException("The ranges list cannot be null or empty.");
        }

        if (rule.CommissionRangeConfigs.Any(r =>
                r is { Type: CommissionType.Percentage, CommissionAmount: < -10 or > 10 }))
        {
            throw new InvalidOperationException(
                "For 'Percentage' CommissionType, the CommissionAmount should be between -10 and 10. Commissions over 1000% are not allowed.");
        }

        if (rule.CommissionRangeConfigs.Count == 1)
        {
            var only = rule.CommissionRangeConfigs[0];
            if (only.RangeStart != 0 || only.RangeEnd != 0)
            {
                throw new InvalidOperationException("In case of one range, both 'From' and 'To' should be 0.");
            }

            if (only.MaxCommission != 0 && only.MaxCommission < only.MinCommission)
            {
                throw new InvalidOperationException("MaxCommission should be greater than or equal to MinCommission.");
            }

            return;
        }

        ValidateEachRange(rule);
    }

    private static void ValidateEachRange(CommissionRule rule)
    {
        var startRule = rule.CommissionRangeConfigs.FirstOrDefault(r => r is { RangeStart: 0, RangeEnd: > 0 });
        if (startRule == null)
        {
            throw new InvalidOperationException("There should be at least one rule where From = 0.");
        }

        if (startRule.MaxCommission != 0 && startRule.MaxCommission < startRule.MinCommission)
        {
            throw new InvalidOperationException("MaxCommission should be greater than or equal to MinCommission.");
        }

        var verifiedRules = 1;
        var lastTo = startRule.RangeEnd;

        while (true)
        {
            var nextRule = rule.CommissionRangeConfigs.FirstOrDefault(r => r.RangeStart == lastTo);
            if (nextRule is null && lastTo != 0)
            {
                throw new InvalidOperationException($"Gap detected. No rule found for 'From = {lastTo}'.");
            }

            if (nextRule is not null && nextRule.RangeStart == nextRule.RangeEnd)
            {
                throw new InvalidOperationException("Invalid rule. 'From' and 'To' cannot be equal.");
            }

            if (nextRule is not null && nextRule.MaxCommission != 0 && nextRule.MaxCommission < nextRule.MinCommission)
            {
                throw new InvalidOperationException("MaxCommission should be greater than or equal to MinCommission.");
            }

            if (lastTo == 0)
            {
                break;
            }

            verifiedRules++;
            lastTo = nextRule!.RangeEnd;
        }

        if (verifiedRules != rule.CommissionRangeConfigs.Count)
        {
            throw new InvalidOperationException("There is some nested or gap ranges in the rules.");
        }
    }
}
