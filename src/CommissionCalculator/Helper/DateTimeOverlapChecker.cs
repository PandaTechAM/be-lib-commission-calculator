using CommissionCalculator.DTO;

namespace CommissionCalculator.Helper;

public static class DateTimeOverlapChecker
{
   public static bool HasOverlap(List<DateTimePair> firstPairs, List<DateTimePair> secondPairs)
   {
      foreach (var firstPair in firstPairs)
      {
         foreach (var secondPair in secondPairs)
         {
            if (IsOverlapping(firstPair, secondPair))
            {
               return true;
            }
         }
      }

      return false;
   }

   private static bool IsOverlapping(DateTimePair firstPair, DateTimePair secondPair)
   {
      return firstPair.StartDate <= secondPair.EndDate && secondPair.StartDate <= firstPair.EndDate;
   }
}