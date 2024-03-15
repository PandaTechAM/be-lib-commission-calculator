using CommissionCalculator.DTO;

namespace CommissionCalculator.Tests;

public class LoadTests
{
    [Fact]
    public void TestLoad()
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

        for (var i = 0; i < 1_000_0; i++)
        {
            var commission = Commission.ComputeCommission(755789, rules);
        }

        Assert.Equal(1, 1);
    }
}