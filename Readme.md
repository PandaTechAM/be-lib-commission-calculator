# 1. Pandatech.CommissionCalculator

## 1.1. Overview

The updated Pandatech.CommissionCalculator is a comprehensive .NET library designed to streamline commission
calculations. With enhanced features supporting both proportional and absolute commission types, the library now offers
more granular control over commission calculations through detailed range configurations. The introduction of
CalculationType in commission rules allows users to specify the nature of the commission calculation explicitly.
Moreover, the library maintains its utility for date range overlap checks with improved efficiency, ensuring commission
period validations remain precise and reliable.

## 1.2. Features

- **Proportional and Absolute Commissions** - Enhanced support for both proportional and absolute commission
  calculations, providing extensive flexibility in defining commission structures.
- **Detailed Commission Range Configurations** - Define commission calculations with precision using
  `CommissionRangeConfigs`, allowing for intricate rule definitions.
- **Rule-Based Tiering** - Define custom rules for various range tiers.
- **Auto-Validation** - Improved validation mechanisms ensure that commission rules are logically consistent and
  well-structured before computation.
- **Strict Rule Coverage** - Guarantees complete domain coverage from 0 to +∞, with validation checks to ensure no gaps
  or overlaps in commission ranges.
- **Configurable** - The library is highly configurable, allowing you to define your own commission ranges and
  commission types.
- **Robust Testing** - Achieves 99% code coverage, ensuring reliability.
- **Optimized Performance** - Delivers high performance, capable of over 5 million calculations per second with 0 ram
  allocation.

## 1.3. Installation

Install the latest version of the Pandatech.CommissionCalculator library via NuGet with the following command:

```bash
Install-Package Pandatech.CommissionCalculator
```

## 1.4. Usage

### 1.4.1. Define Commission Rule

```csharp
using CommissionCalculator.DTO;

var rules = new CommissionRule
{
    CalculationType = CalculationType.Proportional,
    DecimalPlace = 4,
    CommissionRangeConfigs = new List<CommissionRangeConfigs>
    {
        new CommissionRule
            {
                RangeStart = 0,
                RangeEnd = 500,
                Type = CommissionType.FlatRate,
                CommissionAmount = 25,
                MinCommission = 0,
                MaxCommission = 0
            },
            new CommissionRule
            {
                RangeStart = 500,
                RangeEnd = 1000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.1m,
                MinCommission = 70,
                MaxCommission = 90
            },
            new CommissionRule
            {
                RangeStart = 1000,
                RangeEnd = 10000,
                Type = CommissionType.Percentage,
                CommissionAmount = 0.2m,
                MinCommission = 250,
                MaxCommission = 1500
            },
            new CommissionRule
            {
                RangeStart = 10000,
                RangeEnd = 0,
                Type = CommissionType.FlatRate,
                CommissionAmount = 2000,
                MinCommission = 0,
                MaxCommission = 0
            }
    }
};
```

### 1.4.2. Compute Commission

A) Standard (principal selects and applies)

```csharp
decimal principalAmount = 1000m;
decimal commission = Commission.ComputeCommission(principalAmount, rule);
```

B) Selector-based (selector selects, principal applies)

Use when the value that determines the rule (e.g., ticket count) is not the amount you apply the commission to (e.g.,
order price).

```csharp
decimal orderPrice = 2000m;
decimal ticketCount = 3m; // selector
decimal commission = Commission.ComputeCommission(orderPrice, ticketCount, rule);
```

> **Notes**
> - The selector-based overload works with `CalculationType.Absolute`.
> - If used with `CalculationType.Proportional`, an `InvalidOperationException` is thrown.
> - `MinCommission/MaxCommission` are taken from the selected range (by selector) and enforced on the commission
    computed from the principal.
> - `FlatRate` returns the flat amount (min/max ignored).

### 1.4.3. Validation

Validate your commission rules to ensure they are consistent and cover the required domain without overlaps or gaps:

```csharp
var isValidRule = CommissionCalculator.ValidateRule(rule);
```

The validation is automatically performed when computing the commission to prevent logical inconsistencies.

### 1.4.4. Validate DateTime Overlap

```csharp
using CommissionCalculator.Helper;
using CommissionCalculator.DTO;

var firstRange = new List<DateTimePair>
{
    new DateTimePair(new DateTime(2024, 1, 1), new DateTime(2024, 1, 10))
};
var secondRange = new List<DateTimePair>
{
    new DateTimePair(new DateTime(2024, 1, 2), new DateTime(2024, 1, 8))
};

bool hasOverlap = DateTimeOverlapChecker.HasOverlap(firstRange, secondRange);
Console.WriteLine(hasOverlap ? "Overlap detected." : "No overlaps.");
```

## 1.5. CommissionRangeConfig Properties

- **RangeStart:** The beginning of the commission rule's range.
- **RangeEnd:** The end of the range (set to 0 for +∞).
- **Type:** Determines if the commission is a flat rate or percentage.
- **CommissionAmount:** The specific commission amount for the range.
- **MinCommission:** The range minimum commission.
- **MaxCommission:** The range maximum commission (0 for +∞).

## 1.6. Conventions

- If you need to specify +∞, set `RangeEnd` or `MaxCommission` to 0. This is for database efficiency.
- Ensure that your rules cover the entire domain `[0, +∞)`. If a rule is missing, an exception is thrown.

## 1.7. Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## 1.8. License

This project is licensed under the MIT License.
