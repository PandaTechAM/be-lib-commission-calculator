namespace CommissionCalculator.DTO;

/// <summary>
///     Determines how a tiered commission is accumulated across ranges.
/// </summary>
public enum CalculationType
{
    /// <summary>Apply only the tier that contains the principal amount.</summary>
    Absolute,

    /// <summary>Sum each tier's contribution up to the principal amount (progressive).</summary>
    Proportional
}
