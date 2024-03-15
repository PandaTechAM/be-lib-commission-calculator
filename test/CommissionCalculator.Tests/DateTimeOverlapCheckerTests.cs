using CommissionCalculator.DTO;
using CommissionCalculator.Helper;

namespace CommissionCalculator.Tests;

public class DateTimeOverlapCheckerTests
{
    [Fact]
    public void NoOverlapExists_ReturnsFalse()
    {
        // Arrange
        var firstPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2024, 1, 1), new DateTime(2024, 1, 10))
        };
        var secondPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2024, 1, 11), new DateTime(2024, 1, 20))
        };

        // Act
        var result = DateTimeOverlapChecker.HasOverlap(firstPairs, secondPairs);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OverlapExistsCase1_ReturnsTrue()
    {
        // Arrange
        var firstPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2024, 1, 1), new DateTime(2024, 1, 10))
        };
        var secondPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2024, 1, 5), new DateTime(2024, 1, 15))
        };

        // Act
        var result = DateTimeOverlapChecker.HasOverlap(firstPairs, secondPairs);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OverlapExistsCase2_ReturnsTrue()
    {
        // Arrange
        var firstPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2024, 1, 1), new DateTime(2024, 1, 10))
        };
        var secondPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2023, 1, 10), new DateTime(2025, 1, 20))
        };

        // Act
        var result = DateTimeOverlapChecker.HasOverlap(firstPairs, secondPairs);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public void OverlapExistsCase3_ReturnsTrue()
    {
        // Arrange
        var firstPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2024, 1, 1), new DateTime(2024, 1, 10))
        };
        var secondPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2024, 1, 2), new DateTime(2024, 1, 8))
        };

        // Act
        var result = DateTimeOverlapChecker.HasOverlap(firstPairs, secondPairs);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OverlapExistsCase4_ReturnsTrue()
    {
        // Arrange
        var firstPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2024, 1, 10), new DateTime(2024, 1, 20))
        };
        var secondPairs = new List<DateTimePair>
        {
            new DateTimePair(new DateTime(2023, 1, 5), new DateTime(2024, 1, 11))
        };

        // Act
        var result = DateTimeOverlapChecker.HasOverlap(firstPairs, secondPairs);

        // Assert
        Assert.True(result);
    }
}