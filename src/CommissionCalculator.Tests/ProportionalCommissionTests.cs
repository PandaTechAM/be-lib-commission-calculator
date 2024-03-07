using CommissionCalculator;
using CommissionCalculator.DTO;

namespace CommissionCalculator.Tests;

public class ProportionalCommissionTests
{
    [Theory]
    [InlineData(120, 25)]
    [InlineData(450, 25)]
    [InlineData(500, 25)]
    [InlineData(520, 27)]
    [InlineData(800, 55)]
    [InlineData(999, 75)]
    [InlineData(1200, 325)]
    [InlineData(10000, 3575)]
    [InlineData(150000, 3575)]
    public void TestProportionalCommission(decimal principal, decimal expected)
    {
        var ranges = new List<CommissionRangeConfigs>
        {
            new CommissionRangeConfigs
            {
                RangeStart = 0, RangeEnd = 500, Type = CommissionType.FlatRate, CommissionAmount = 25,
                MinCommission = 0, MaxCommission = 0
            },
            new CommissionRangeConfigs
            {
                RangeStart = 500, RangeEnd = 1000, Type = CommissionType.Percentage, CommissionAmount = 0.1m,
                MinCommission = 0, MaxCommission = 0
            },
            new CommissionRangeConfigs
            {
                RangeStart = 1000, RangeEnd = 10000, Type = CommissionType.Percentage, CommissionAmount = 0.2m,
                MinCommission = 250, MaxCommission = 1500
            },
            new CommissionRangeConfigs
            {
                RangeStart = 10000, RangeEnd = 0, Type = CommissionType.FlatRate, CommissionAmount = 2000,
                MinCommission = 0, MaxCommission = 0
            }
        };
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };

        var commission = Commission.ComputeCommission(principal, rules);

        Assert.Equal(expected, commission);
    }
}