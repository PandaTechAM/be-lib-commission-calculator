using CommissionCalculator.DTO;

namespace CommissionCalculator.Helper;

/// <summary>
///     Utilities for detecting overlaps between sets of date-time intervals.
/// </summary>
public static class DateTimeOverlapChecker
{
    /// <summary>
    ///     Return true if any interval in the first set overlaps any interval in the second.
    /// </summary>
    public static bool HasOverlap(List<DateTimePair> firstPairs, List<DateTimePair> secondPairs)
    {
        return firstPairs.Any(firstPair => secondPairs.Any(secondPair => IsOverlapping(firstPair, secondPair)));
    }

    private static bool IsOverlapping(DateTimePair firstPair, DateTimePair secondPair)
    {
        return firstPair.StartDate <= secondPair.EndDate && secondPair.StartDate <= firstPair.EndDate;
    }
}
