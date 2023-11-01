namespace CommissionCalculator;

public class CommissionCalculator
{
    private static readonly decimal DecimalEpsilon; //smallest decimal value that is greater than zero

    static CommissionCalculator()
    {
        DecimalEpsilon = 1e-28m;
    }

    public static decimal ComputeCommission(decimal principalAmount, List<CommissionRule> rules,
        bool isProportional, int decimalPlaces = 4)
    {
        decimal commission;

        if (isProportional)
        {
            commission = CalculateProportionalCommission(principalAmount, rules);
            return Math.Round(commission, decimalPlaces);
        }

        commission = CalculateAbsoluteCommission(principalAmount, rules);
        return Math.Round(commission, decimalPlaces);
    }

    private static decimal CalculateAbsoluteCommission(decimal principalAmount, List<CommissionRule> rules)
    {
        rules = ConvertCommissionRules(rules);

        var rule = rules.FirstOrDefault(r => principalAmount >= r.RangeStart && principalAmount < r.RangeEnd);

        return ComputeRuleCommission(rule!.Type, rule.CommissionAmount, rule.MinCommission, rule.MaxCommission,
            principalAmount);
    }

    private static decimal CalculateProportionalCommission(decimal principalAmount, List<CommissionRule> rules)
    {
        rules = ConvertCommissionRules(rules);

        decimal commission = 0;


        foreach (var rule in rules)
        {
            if (principalAmount >= rule.RangeStart && principalAmount < rule.RangeEnd)
            {
                var portionOfPrincipal = principalAmount - rule.RangeStart;

                commission += ComputeRuleCommission(rule.Type, rule.CommissionAmount, rule.MinCommission,
                    rule.MaxCommission,
                    portionOfPrincipal);
            }

            if (principalAmount >= rule.RangeEnd)
            {
                var portionOfPrincipal = rule.RangeEnd - rule.RangeStart - DecimalEpsilon;

                commission += ComputeRuleCommission(rule.Type, rule.CommissionAmount, rule.MinCommission,
                    rule.MaxCommission,
                    portionOfPrincipal);
            }
        }

        return commission;
    }

    private static decimal ComputeRuleCommission(CommissionType commissionType, decimal commission, decimal minimum,
        decimal maximum,
        decimal principalAmount)
    {
        if (commissionType == CommissionType.FlatRate) return commission;
        var computedCommission = principalAmount * commission;
        if (computedCommission < minimum) return minimum;
        if (computedCommission > maximum) return maximum;
        return computedCommission;
    }

    private static List<CommissionRule> ConvertCommissionRules(List<CommissionRule> rules)
    {
        ValidateCommissionRules(rules);

        return rules.Select(rule => new CommissionRule
            {
                RangeStart = rule.RangeStart,
                RangeEnd = rule.RangeEnd == 0 ? decimal.MaxValue : rule.RangeEnd,
                Type = rule.Type,
                CommissionAmount = rule.CommissionAmount,
                MinCommission = rule.MinCommission,
                MaxCommission = rule.MaxCommission == 0 ? decimal.MaxValue : rule.MaxCommission
            })
            .ToList();
    }


    public static void ValidateCommissionRules(IReadOnlyCollection<CommissionRule> rules)
    {
        if (rules == null || !rules.Any())
        {
            throw new ArgumentException("The rules list cannot be null or empty.");
        }

        if (rules.Any(r => r is { Type: CommissionType.Percentage, CommissionAmount: < -1 or > 1 }))
        {
            throw new InvalidOperationException(
                "For 'Percentage' CommissionType, the CommissionAmount should be between -1 and 1.");
        }

        var duplicates = rules.GroupBy(r => r.RangeStart).Where(g => g.Count() > 1).ToList();
        
        if (duplicates.Any())
        {
            throw new InvalidOperationException(
                $"There are duplicate rules with the same 'From' value: {string.Join(", ", duplicates.Select(d => d.Key))}.");
        }

        if (!rules.All(checking => checking.RangeStart == 0 || rules.Any(r => r.RangeEnd == checking.RangeStart) ))
        {
            throw new InvalidOperationException(
                "There are rules with nested ranges. Please check the rules.");
        }
        
        var startRule = rules.FirstOrDefault(r => r.RangeStart == 0 && r.RangeEnd != 0);
        if (startRule == null)
        {
            throw new InvalidOperationException("There should be at least one rule where From = 0.");
        }

        var lastTo = startRule.RangeEnd;

        while (true)
        {
            var nextRule = rules.FirstOrDefault(r => r.RangeStart == lastTo);
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

            lastTo = nextRule!.RangeEnd;
        }
    }
}