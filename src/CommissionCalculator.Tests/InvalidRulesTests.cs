using CommissionCalculator.DTO;

namespace CommissionCalculator.Tests;

public class InvalidRulesTests
{
    [Fact]
    public void TestPercentageValidation()
    {
        var ranges = new List<CommissionRangeConfigs>
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
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };

        var isValid = Commission.ValidateRule(rules);

        Assert.False(isValid);
    }
    
    [Fact]
    public void TestRangeEqualValidation()
    {
        var ranges = new List<CommissionRangeConfigs>
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
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };

        var isValid = Commission.ValidateRule(rules);

        Assert.False(isValid);
    }
    
    [Fact]
    public void TestRangeOverlappingValidation()
    {
        var ranges = new List<CommissionRangeConfigs>
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
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };

        var isValid = Commission.ValidateRule(rules);

        Assert.False(isValid);
    }

    
    [Fact]
    public void TestWrongRangeStart()
    {
        var ranges = new List<CommissionRangeConfigs>
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
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };

        var isValid = Commission.ValidateRule(rules);

        Assert.False(isValid);
    }
    
    [Fact]
    public void TestWrongRangeEnd()
    {
        var ranges = new List<CommissionRangeConfigs>
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
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };
        
        var isValid = Commission.ValidateRule(rules);

        Assert.False(isValid);
    }
    
    [Fact]
    public void TestGap()
    {
        var ranges = new List<CommissionRangeConfigs>
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
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };

        var isValid = Commission.ValidateRule(rules);

        Assert.False(isValid);
    }
    
    [Fact]
    public void TestDuplicates()
    {
        var ranges = new List<CommissionRangeConfigs>
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
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };

        var isValid = Commission.ValidateRule(rules);

        Assert.False(isValid);
    }
    
    
        
    [Fact]
    public void TestNestedRange()
    {
        var ranges = new List<CommissionRangeConfigs>
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
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };

        var isValid = Commission.ValidateRule(rules);

        Assert.False(isValid);
    }
    
    [Fact]
    public void TestNested2Range()
    {
        var ranges = new List<CommissionRangeConfigs>
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
        
        var rules = new CommissionRule
        {
            CalculationType = CalculationType.Proportional,
            DecimalPlace = 0,
            CommissionRangeConfigs = ranges
        };

        var isValid = Commission.ValidateRule(rules);

        Assert.False(isValid);
    }
}
