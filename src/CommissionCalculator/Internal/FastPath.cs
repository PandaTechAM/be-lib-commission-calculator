using CommissionCalculator.DTO;

namespace CommissionCalculator.Internal;

internal static class FastPath
{
   // Binary search in normalized, contiguous, non-overlapping ranges
   public static int FindRangeIndex(NormalizedCommissionRule nr, decimal value)
   {
      var arr = nr.Ranges;
      int lo = 0, hi = arr.Length - 1;

      while (lo <= hi)
      {
         var mid = lo + ((hi - lo) >> 1);
         var r = arr[mid];

         if (value < r.Start)
         {
            hi = mid - 1;
            continue;
         }

         if (value < r.End)
         {
            return mid;
         }

         lo = mid + 1;
      }

      // Shouldn’t happen after validation; clamp to be safe
      return lo <= 0 ? 0 : lo >= arr.Length ? arr.Length - 1 : lo;
   }

   // Same semantics as your original ComputeRangeCommission
   public static decimal ComputeRangeCommission(CommissionType commissionType,
      decimal commission,
      decimal minimum,
      decimal maximum,
      decimal principalPortion)
   {
      if (commissionType == CommissionType.FlatRate)
      {
         return commission;
      }

      var c = principalPortion * commission;
      if (c < minimum)
      {
         return minimum;
      }

      return c > maximum ? maximum : c;
   }
}