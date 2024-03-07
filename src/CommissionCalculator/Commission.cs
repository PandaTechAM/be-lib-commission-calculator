using CommissionCalculator.DTO;

namespace CommissionCalculator;

public static class Commission
{
    private const decimal DecimalEpsilon = 1e-28M; //smallest decimal value that is greater than zero

    public static decimal ComputeCommission(decimal principalAmount, CommissionRule rule)
    {
        decimal commission;

        if (rule.CalculationType == CalculationType.Proportional)
        {
            commission = CalculateProportionalCommission(principalAmount, rule);
            return Math.Round(commission, rule.DecimalPlace);
        }

        commission = CalculateAbsoluteCommission(principalAmount, rule);
        return Math.Round(commission, rule.DecimalPlace);
    }

    private static decimal CalculateAbsoluteCommission(decimal principalAmount, CommissionRule rule)
    {
        rule = ConvertCommissionRanges(rule);

        var range = rule.CommissionRangeConfigs.FirstOrDefault(r =>
            principalAmount >= r.RangeStart && principalAmount < r.RangeEnd);

        return ComputeRangeCommission(range!.Type, range.CommissionAmount, range.MinCommission, range.MaxCommission,
            principalAmount);
    }

    private static decimal CalculateProportionalCommission(decimal principalAmount, CommissionRule rule)
    {
        rule = ConvertCommissionRanges(rule);

        decimal commission = 0;


        foreach (var range in rule.CommissionRangeConfigs)
        {
            if (principalAmount >= range.RangeStart && principalAmount < range.RangeEnd)
            {
                var portionOfPrincipal = principalAmount - range.RangeStart;

                commission += ComputeRangeCommission(range.Type, range.CommissionAmount, range.MinCommission,
                    range.MaxCommission,
                    portionOfPrincipal);
            }

            if (principalAmount < range.RangeEnd) continue;
            {
                var portionOfPrincipal = range.RangeEnd - range.RangeStart - DecimalEpsilon;

                commission += ComputeRangeCommission(range.Type, range.CommissionAmount, range.MinCommission,
                    range.MaxCommission,
                    portionOfPrincipal);
            }
        }

        return commission;
    }

    private static decimal ComputeRangeCommission(CommissionType commissionType, decimal commission, decimal minimum,
        decimal maximum,
        decimal principalAmount)
    {
        if (commissionType == CommissionType.FlatRate) return commission;
        var computedCommission = principalAmount * commission;
        if (computedCommission < minimum) return minimum;
        if (computedCommission > maximum) return maximum;
        return computedCommission;
    }

    private static CommissionRule ConvertCommissionRanges(CommissionRule rule)
    {
        ValidateCommissionRule(rule);

        var convertedRanges = rule.CommissionRangeConfigs.Select(c => new CommissionRangeConfigs
            {
                RangeStart = c.RangeStart,
                RangeEnd = c.RangeEnd == 0 ? decimal.MaxValue : c.RangeEnd,
                Type = c.Type,
                CommissionAmount = c.CommissionAmount,
                MinCommission = c.MinCommission,
                MaxCommission = c.MaxCommission == 0 ? decimal.MaxValue : c.MaxCommission
            })
            .ToList();
        return new CommissionRule
        {
            CalculationType = rule.CalculationType,
            DecimalPlace = rule.DecimalPlace,
            CommissionRangeConfigs = convertedRanges
        };
    }


    public static void ValidateCommissionRule(CommissionRule rule)
    {
        if (rule == null || rule.CommissionRangeConfigs.Count == 0)
        {
            throw new ArgumentException("The ranges list cannot be null or empty.");
        }

        if (rule.CommissionRangeConfigs.Any(
                r => r is { Type: CommissionType.Percentage, CommissionAmount: < -1 or > 1 }))
        {
            throw new InvalidOperationException(
                "For 'Percentage' CommissionType, the CommissionAmount should be between -1 and 1.");
        }

        var startRule = rule.CommissionRangeConfigs.FirstOrDefault(r => r.RangeStart == 0 && r.RangeEnd != 0);
        if (startRule == null)
        {
            throw new InvalidOperationException("There should be at least one rule where From = 0.");
        }

        var verifiedRules = 1;

        var lastTo = startRule.RangeEnd;

        while (true)
        {
            var nextRule = rule.CommissionRangeConfigs.FirstOrDefault(r => r.RangeStart == lastTo);
            if (nextRule == null && lastTo != 0)
            {
                throw new InvalidOperationException($"Gap detected. No rule found for 'From = {lastTo}'.");
            }

            if (nextRule != null && nextRule.RangeStart == nextRule.RangeEnd)
            {
                throw new InvalidOperationException($"Invalid rule. 'From' and 'To' cannot be equal.");
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