using CommissionCalculator.DTO;

namespace CommissionCalculator.Helper;

public static class DateTimeOverlapChecker
{
   public static bool HasOverlap(List<DateTimePair> firstPairs, List<DateTimePair> secondPairs)
   {
      return firstPairs.Any(firstPair => secondPairs.Any(secondPair => IsOverlapping(firstPair, secondPair)));
   }

   private static bool IsOverlapping(DateTimePair firstPair, DateTimePair secondPair)
   {
      return firstPair.StartDate <= secondPair.EndDate && secondPair.StartDate <= firstPair.EndDate;
   }
}