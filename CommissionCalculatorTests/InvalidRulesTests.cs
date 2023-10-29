using CommissionCalculator;

namespace CommissionCalculatorTests;

public class InvalidRulesTests
{
    [Fact]
    public void TestPercentageValidation()
    {
        var rules = new List<CommissionRule>
        {
            new()
            {
                RangeStart = 0,
                RangeEnd = 1000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.1m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 1000,
                RangeEnd = 5000,
                Type = CommissionType.Percentage,
                CommissionAmount = 10.05m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 5000,
                RangeEnd = 7000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.03m,
                MinCommission = 0,
                MaxCommission = 0
            }
        };

        Assert.Throws<InvalidOperationException>(() => CommissionCalculator.CommissionCalculator.ValidateCommissionRules(rules));
    }
    
    [Fact]
    public void TestRangeEqualValidation()
    {
        var rules = new List<CommissionRule>
        {
            new()
            {
                RangeStart = 0,
                RangeEnd = 1000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.1m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 1000,
                RangeEnd = 1000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.05m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 5000,
                RangeEnd = 7000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.03m,
                MinCommission = 0,
                MaxCommission = 0
            }
        };

        Assert.Throws<InvalidOperationException>(() => CommissionCalculator.CommissionCalculator.ValidateCommissionRules(rules));
    }
    
    [Fact]
    public void TestWrongRangeStart()
    {
        var rules = new List<CommissionRule>
        {
            new()
            {
                RangeStart = -1,
                RangeEnd = 1000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.1m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 1000,
                RangeEnd = 5000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.05m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 5000,
                RangeEnd = 7000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.03m,
                MinCommission = 0,
                MaxCommission = 0
            }
        };

        Assert.Throws<InvalidOperationException>(() => CommissionCalculator.CommissionCalculator.ValidateCommissionRules(rules));
    }
    
    [Fact]
    public void TestWrongRangeEnd()
    {
        var rules = new List<CommissionRule>
        {
            new()
            {
                RangeStart = 0,
                RangeEnd = 1000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.1m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 1000,
                RangeEnd = 5000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.05m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 5000,
                RangeEnd = 7000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.03m,
                MinCommission = 0,
                MaxCommission = 0
            }
        };

        Assert.Throws<InvalidOperationException>(() => CommissionCalculator.CommissionCalculator.ValidateCommissionRules(rules));
    }
}