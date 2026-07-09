using CommissionCalculator.DTO;

namespace CommissionCalculator.Tests;

public class NegativeDecimalPlaceRoundingTests
{
    [Theory]
    [InlineData(149, 100)]
    [InlineData(150, 200)]
    [InlineData(149.5, 100)]
    public void PrincipalBasedCommission_RoundsNegativeDecimalPlaceToNearestHundred(
        decimal commissionAmount,
        decimal expected)
    {
        var rule = CreateFlatRateRule(commissionAmount, -2);

        var commission = Commission.ComputeCommission(1000, rule);

        Assert.Equal(expected, commission);
    }

    [Fact]
    public void SelectorBasedCommission_RoundsNegativeDecimalPlaceToNearestHundred()
    {
        var rule = CreateFlatRateRule(150, -2);

        var commission = Commission.ComputeCommission(1000, 10, rule);

        Assert.Equal(200, commission);
    }

    [Fact]
    public void PrincipalBasedCommission_KeepsPositiveDecimalPlaceBehavior()
    {
        var rule = CreateFlatRateRule(149.555M, 2);

        var commission = Commission.ComputeCommission(1000, rule);

        Assert.Equal(149.56M, commission);
    }

    private static CommissionRule CreateFlatRateRule(decimal commissionAmount, short decimalPlace)
    {
        return new CommissionRule
        {
            CalculationType = CalculationType.Absolute,
            DecimalPlace = decimalPlace,
            CommissionRangeConfigs =
            [
                new CommissionRangeConfigs
                {
                    RangeStart = 0,
                    RangeEnd = 0,
                    Type = CommissionType.FlatRate,
                    CommissionAmount = commissionAmount,
                    MinCommission = 0,
                    MaxCommission = 0
                }
            ]
        };
    }
}
