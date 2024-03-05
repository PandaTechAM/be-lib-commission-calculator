using CommissionCalculator;
using CommissionCalculator.DTO;

namespace CommissionCalculator.Tests;

public class AbsoluteCommissionTests
{
    [Theory]
    [InlineData(120, 25)]
    [InlineData(450, 25)]
    [InlineData(500, 70)]
    [InlineData(520, 70)]
    [InlineData(800, 80)]
    [InlineData(999, 90)]
    [InlineData(1200, 250)]
    [InlineData(10000, 2000)]
    [InlineData(150000, 2000)]
    public void TestAbsoluteCommission(decimal principal, decimal expected)
    {
        var rules = new List<CommissionRule>
        {
            new CommissionRule
            {
                RangeStart = 10000, RangeEnd = 0, Type = CommissionType.FlatRate, CommissionAmount = 2000,
                MinCommission = 0, MaxCommission = 0
            },
            new CommissionRule
            {
                RangeStart = 0, RangeEnd = 500, Type = CommissionType.FlatRate, CommissionAmount = 25,
                MinCommission = 0, MaxCommission = 0
            },
            new CommissionRule
            {
                RangeStart = 500, RangeEnd = 1000, Type = CommissionType.Percentage, CommissionAmount = 0.1m,
                MinCommission = 70, MaxCommission = 90
            },
            new CommissionRule
            {
                RangeStart = 1000, RangeEnd = 10000, Type = CommissionType.Percentage, CommissionAmount = 0.2m,
                MinCommission = 250, MaxCommission = 1500
            },

        };

        var commission = Commission.ComputeCommission(principal, rules, false, 0);

        Assert.Equal(expected, commission);
    }
}