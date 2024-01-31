using CommissionCalculator;

namespace CommissionCalculator.Tests;

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

        Assert.Throws<InvalidOperationException>(() => Commission.ValidateCommissionRules(rules));
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

        Assert.Throws<InvalidOperationException>(() => Commission.ValidateCommissionRules(rules));
    }
    
    [Fact]
    public void TestRangeOverlappingValidation()
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
                RangeStart = 500,
                RangeEnd = 5000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.05m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 5000,
                RangeEnd = 0,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.03m,
                MinCommission = 0,
                MaxCommission = 0
            }
        };

        Assert.Throws<InvalidOperationException>(() => Commission.ValidateCommissionRules(rules));
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

        Assert.Throws<InvalidOperationException>(() => Commission.ValidateCommissionRules(rules));
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

        Assert.Throws<InvalidOperationException>(() => Commission.ValidateCommissionRules(rules));
    }
    
    [Fact]
    public void TestGap()
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
                RangeStart = 2000,
                RangeEnd = 5000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.05m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 5000,
                RangeEnd = 0,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.03m,
                MinCommission = 0,
                MaxCommission = 0
            }
        };

        Assert.Throws<InvalidOperationException>(() => Commission.ValidateCommissionRules(rules));
    }
    
    [Fact]
    public void TestDuplicates()
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
                RangeEnd = 0,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.03m,
                MinCommission = 0,
                MaxCommission = 0
            }
        };

        Assert.Throws<InvalidOperationException>(() => Commission.ValidateCommissionRules(rules));
    }
    
    
        
    [Fact]
    public void TestNestedRange()
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
                RangeStart = 1580,
                RangeEnd = 4000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.05m,
                MinCommission = 0,
                MaxCommission = 0
            },
            new()
            {
                RangeStart = 5000,
                RangeEnd = 0,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.03m,
                MinCommission = 0,
                MaxCommission = 0
            }
        };

        Assert.Throws<InvalidOperationException>(() => Commission.ValidateCommissionRules(rules));
    }
    
    [Fact]
    public void TestNested2Range()
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
                RangeStart = 1000,
                RangeEnd = 7000,
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
            },
            new()
            {
                RangeStart = 7000,
                RangeEnd = 0,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.03m,
                MinCommission = 0,
                MaxCommission = 0
            }
        };

        Assert.Throws<InvalidOperationException>(() => Commission.ValidateCommissionRules(rules));
    }
}
