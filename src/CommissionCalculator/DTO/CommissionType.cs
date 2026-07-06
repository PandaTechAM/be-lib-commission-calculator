namespace CommissionCalculator.DTO;

/// <summary>
///     Determines how a range's commission amount is interpreted.
/// </summary>
public enum CommissionType
{
    /// <summary>A fixed commission amount regardless of the principal.</summary>
    FlatRate,

    /// <summary>A rate multiplied by the principal, bounded by the range's min/max commission.</summary>
    Percentage
}
