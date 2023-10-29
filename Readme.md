# Pandatech.CommissionCalculator

## Overview

The Pandatech.CommissionCalculator is a .NET library that simplifies the commission calculation process, offering robust
features and a flexible configuration to handle a multitude of scenarios. This package allows for both proportional and
absolute commission calculations, rule-based tiering, and even imposes validation rules to ensure logical consistency.

## Features

* **Proportional and Absolute Commissions** - The library supports both proportional and absolute commission
  calculations. Choose between proportional or absolute commission types for flexibility.
* **Rule-Based Tiering** - Define custom rules for various range tiers.
* **Auto-Validation** - The package performs automatic validation on your commission rules. If a rule is invalid, an
  exception is thrown.
* **Strict Rule Coverage** - Ensures your rule set covers the entire domain from 0 to +∞.
* **Flexible Configuration** - The library is highly configurable, allowing you to define your own commission rules,
  commission types, and validation rules.
* **High Test Coverage** - The library is thoroughly tested, with 99% code coverage.
* **Highly Performant** - The library is highly performant, with at least 1,500,000 calculations per second.

## Installation

The Pandatech.CommissionCalculator library is available on NuGet. To install, run the following command in the Package
Manager Console:

```bash
Install-Package Pandatech.CommissionCalculator
```

## Usage

### Initialize Commission Rules

```csharp
var rules = new List<CommissionRule>
{
    // Add your commission rules here
    var rules = new List<CommissionRule>
        {
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
            new CommissionRule
            {
                RangeStart = 10000, RangeEnd = 0, Type = CommissionType.FlatRate, CommissionAmount = 2000,
                MinCommission = 0, MaxCommission = 0
            }
        };
};
```

### Compute Commission

```csharp
decimal principalAmount = 1000m;
bool isProportional = true;
int decimalPlaces = 4;

decimal commission = CommissionCalculator.ComputeCommission(principalAmount, rules, isProportional, decimalPlaces);
```

### Validation

```csharp
CommissionCalculator.ValidateCommissionRules(rules);
```

Please note that the validation method is automatically called when the ComputeCommission method is invoked. If a rule
is invalid, an exception is thrown.

## CommissionRule Properties

| Property         | Type           | Description                                                               |
|------------------|----------------|---------------------------------------------------------------------------|
| RangeStart       | decimal        | The start range for this rule.                                            |
| RangeEnd         | decimal        | The end range for this rule (0 means infinity).                           |
| Type             | CommissionType | Enum that specifies whether the commission type is FlatRate or Percentage |
| CommissionAmount | decimal        | The commission amount for this rule.                                      |
| MinCommission    | decimal        | The minimum commission for this rule.                                     |
| MaxCommission    | decimal        | The maximum commission for this rule (0 means infinity).                  |

## Conventions

* If you need to specify +∞, set `RangeEnd` or `MaxCommission` to 0. This is for database efficiency.
* Ensure that your rules cover the entire domain `[0, +∞)`. If a rule is missing, an exception is thrown.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License.