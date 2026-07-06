namespace CommissionCalculator.DTO;

/// <summary>
///     An immutable start/end date-time interval.
/// </summary>
public class DateTimePair(DateTime startDate, DateTime endDate)
{
    /// <summary>The start of the interval.</summary>
    public DateTime StartDate { get; } = startDate;

    /// <summary>The end of the interval.</summary>
    public DateTime EndDate { get; } = endDate;
}
