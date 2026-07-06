namespace CommissionCalculator.DTO;

/// <summary>
///     Configuration for a single commission tier: its bounds, type, amount, and min/max commission clamps.
/// </summary>
public class CommissionRangeConfigs
{
    /// <summary>Inclusive lower bound of the range.</summary>
    public decimal RangeStart { get; set; }

    /// <summary>Exclusive upper bound of the range; 0 means infinity.</summary>
    public decimal RangeEnd { get; set; } //0 means infinity

    /// <summary>How the commission amount is interpreted (flat rate or percentage).</summary>
    public CommissionType Type { get; set; }

    /// <summary>Flat amount or percentage rate applied within this range.</summary>
    public decimal CommissionAmount { get; set; }

    /// <summary>Lower clamp applied to the computed commission.</summary>
    public decimal MinCommission { get; set; }

    /// <summary>Upper clamp applied to the computed commission; 0 means infinity.</summary>
    public decimal MaxCommission { get; set; } //0 means infinity
}
