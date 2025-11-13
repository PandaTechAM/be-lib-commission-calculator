using CommissionCalculator.DTO;

namespace CommissionCalculator.Tests;

public class SelectorBasedAbsoluteCommissionTests
{
   private static CommissionRule TicketCountRule =>
      new()
      {
         CalculationType = CalculationType.Absolute,
         DecimalPlace = 0,
         CommissionRangeConfigs =
         [
            new CommissionRangeConfigs
            {
               RangeStart = 0,
               RangeEnd = 2, // [0,2)
               Type = CommissionType.FlatRate,
               CommissionAmount = 50,
               MinCommission = 0,
               MaxCommission = 0
            },

            new CommissionRangeConfigs
            {
               RangeStart = 2,
               RangeEnd = 4, // [2,4)
               Type = CommissionType.Percentage,
               CommissionAmount = 0.10m,
               MinCommission = 0,
               MaxCommission = 0
            },

            new CommissionRangeConfigs
            {
               RangeStart = 4,
               RangeEnd = 0, // [4, +∞)
               Type = CommissionType.FlatRate,
               CommissionAmount = 100,
               MinCommission = 0,
               MaxCommission = 0
            }
         ]
      };

   [Theory]
   // A) Core absolute: select by ticket count, apply on principal
   [InlineData(2000, 3, 200)] // selector 3 -> 10% of 2000
   [InlineData(200, 1.5, 50)] // selector 1.5 -> flat 50
   [InlineData(500, 5, 100)] // selector 5 -> flat 100

   // B) Boundaries
   [InlineData(1000, 2, 100)] // selector == RangeStart(2) of [2,4) => 10% of 1000
   [InlineData(1000, 3.9999, 100)] // near end of [2,4) => 10% of 1000
   [InlineData(1000, 4, 100)] // selector == 4 -> next range [4,∞) flat 100
   public void SelectorAbsolute_Computes_As_Expected(decimal principal, decimal selector, decimal expected)
   {
      var result = Commission.ComputeCommission(principal, selector, TicketCountRule);
      Assert.Equal(expected, result);
   }

   [Fact]
   public void SelectorWithProportionalRule_Throws()
   {
      var proportionalRule = new CommissionRule
      {
         CalculationType = CalculationType.Proportional,
         DecimalPlace = 2,
         CommissionRangeConfigs =
         [
            new CommissionRangeConfigs
            {
               RangeStart = 0,
               RangeEnd = 0,
               Type = CommissionType.Percentage,
               CommissionAmount = 0.05m,
               MinCommission = 0,
               MaxCommission = 0
            }
         ]
      };

      Assert.Throws<InvalidOperationException>(() =>
         Commission.ComputeCommission(1000m, 3m, proportionalRule));
   }
}