namespace CommissionCalculator.DTO;

/// <summary>
///     A commission rule: the calculation model, rounding precision, and the set of tier configurations.
/// </summary>
public class CommissionRule
{
    /// <summary>Whether tiers are applied absolutely or proportionally.</summary>
    public CalculationType CalculationType { get; set; }

    /// <summary>Number of decimal places the resulting commission is rounded to.</summary>
    public short DecimalPlace { get; set; } = 4;

    /// <summary>The tier configurations that make up this rule.</summary>
    public List<CommissionRangeConfigs> CommissionRangeConfigs { get; set; } = null!;
}
