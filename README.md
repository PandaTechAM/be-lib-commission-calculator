# Pandatech.CommissionCalculator

High-performance commission calculation engine for .NET 8+ supporting proportional and absolute commission models with tiered ranges, min/max constraints, and automatic validation.

## Installation

```bash
dotnet add package Pandatech.CommissionCalculator
```

## Quick Start

### Define Commission Rule

```csharp
using CommissionCalculator.DTO;

var rule = new CommissionRule
{
    CalculationType = CalculationType.Proportional,
    DecimalPlace = 4,
    CommissionRangeConfigs = new List<CommissionRangeConfigs>
    {
        new()
        {
            RangeStart = 0,
            RangeEnd = 500,
            Type = CommissionType.FlatRate,
            CommissionAmount = 25,
            MinCommission = 0,
            MaxCommission = 0
        },
        new()
        {
            RangeStart = 500,
            RangeEnd = 1000,
            Type = CommissionType.Percentage,
            CommissionAmount = 0.1m,
            MinCommission = 70,
            MaxCommission = 90
        },
        new()
        {
            RangeStart = 1000,
            RangeEnd = 10000,
            Type = CommissionType.Percentage,
            CommissionAmount = 0.2m,
            MinCommission = 250,
            MaxCommission = 1500
        },
        new()
        {
            RangeStart = 10000,
            RangeEnd = 0, // 0 = infinity
            Type = CommissionType.FlatRate,
            CommissionAmount = 2000,
            MinCommission = 0,
            MaxCommission = 0
        }
    }
};
```

### Calculate Commission

**Standard (principal-based):**

```csharp
decimal principal = 1000m;
decimal commission = Commission.ComputeCommission(principal, rule);
// Result: 25 (flat) + 70 (min enforced) = 95
```

**Selector-based (different selection and application values):**

```csharp
decimal orderPrice = 2000m;     // Amount to calculate commission on
decimal ticketCount = 3m;       // Value to select the range

// Select range based on ticketCount, apply commission to orderPrice
decimal commission = Commission.ComputeCommission(orderPrice, ticketCount, rule);
```

**Use case:** When the value that determines which range to use (e.g., ticket quantity) differs from the amount you calculate commission on (e.g., order total).

## Calculation Types

### Proportional (Tiered)

Commission accumulates across tiers as the amount increases:

```csharp
var proportional = new CommissionRule
{
    CalculationType = CalculationType.Proportional,
    CommissionRangeConfigs = new List<CommissionRangeConfigs>
    {
        new() { RangeStart = 0, RangeEnd = 100, Type = CommissionType.Percentage, CommissionAmount = 0.05m },
        new() { RangeStart = 100, RangeEnd = 0, Type = CommissionType.Percentage, CommissionAmount = 0.03m }
    }
};

// For $200:
// - First $100: $100 * 5% = $5
// - Next $100: $100 * 3% = $3
// Total: $8
```

### Absolute (Flat per tier)

Only the matching tier's commission applies:

```csharp
var absolute = new CommissionRule
{
    CalculationType = CalculationType.Absolute,
    CommissionRangeConfigs = new List<CommissionRangeConfigs>
    {
        new() { RangeStart = 0, RangeEnd = 100, Type = CommissionType.FlatRate, CommissionAmount = 10 },
        new() { RangeStart = 100, RangeEnd = 0, Type = CommissionType.FlatRate, CommissionAmount = 25 }
    }
};

// For $200: $25 (uses second tier only)
```

## Range Configuration

### CommissionRangeConfigs Properties

| Property | Type | Description |
|----------|------|-------------|
| `RangeStart` | decimal | Start of range (inclusive) |
| `RangeEnd` | decimal | End of range (exclusive), **0 = infinity** |
| `Type` | CommissionType | `FlatRate` or `Percentage` |
| `CommissionAmount` | decimal | Commission value (flat amount or percentage like 0.1 for 10%) |
| `MinCommission` | decimal | Minimum commission for this range |
| `MaxCommission` | decimal | Maximum commission for this range, **0 = infinity** |

### CommissionType

- **FlatRate**: Returns `CommissionAmount` directly (min/max ignored)
- **Percentage**: Calculates `principal × CommissionAmount`, clamped by min/max

### Percentage Limits

For safety, percentage commissions are limited to ±1000%:

```csharp
// ✅ Valid: 10% = 0.1
CommissionAmount = 0.1m

// ❌ Invalid: 1500% = 15 (throws exception)
CommissionAmount = 15m
```

## Validation

### Automatic Validation

Validation occurs automatically on first use:

```csharp
decimal commission = Commission.ComputeCommission(1000m, rule);
// Validates rule, caches normalized version, computes result
```

### Manual Validation

```csharp
bool isValid = Commission.ValidateRule(rule);
```

### Validation Rules

✅ **Required:**
- At least one range starting at 0
- Ranges must be contiguous (no gaps)
- Ranges must cover [0, ∞)
- `MaxCommission ≥ MinCommission` (when MaxCommission ≠ 0)

❌ **Forbidden:**
- Overlapping ranges
- Gaps in coverage
- Equal `RangeStart` and `RangeEnd` (except single range: both 0)
- Percentage commission outside ±1000% (±10 as decimal)

## DateTime Overlap Checker

Utility for validating commission period overlaps:

```csharp
using CommissionCalculator.Helper;
using CommissionCalculator.DTO;

var period1 = new List<DateTimePair>
{
    new(new DateTime(2024, 1, 1), new DateTime(2024, 1, 10))
};

var period2 = new List<DateTimePair>
{
    new(new DateTime(2024, 1, 5), new DateTime(2024, 1, 15))
};

bool hasOverlap = DateTimeOverlapChecker.HasOverlap(period1, period2);
// Result: true
```

## Performance

- **5+ million calculations/second** (after initial validation/normalization)
- **Zero allocations** per calculation (after cache hit)
- **Binary search** for range lookup: O(log n)
- **Prefix sum optimization** for proportional calculations
- **ConditionalWeakTable** caching of normalized rules

**Performance characteristics:**
```
First calculation: ~50μs (validation + normalization + computation)
Subsequent calculations: ~200ns (cache hit + binary search + math)
Memory: ~1KB per cached rule
```

## Advanced Usage

### Single Range (Flat Commission)

```csharp
var flatRule = new CommissionRule
{
    CalculationType = CalculationType.Absolute,
    CommissionRangeConfigs = new List<CommissionRangeConfigs>
    {
        new()
        {
            RangeStart = 0,
            RangeEnd = 0, // Special case: both 0 for single range
            Type = CommissionType.FlatRate,
            CommissionAmount = 50,
            MinCommission = 0,
            MaxCommission = 0
        }
    }
};

// Any amount: $50 commission
```

### Progressive Percentage with Caps

```csharp
var capped = new CommissionRule
{
    CalculationType = CalculationType.Proportional,
    CommissionRangeConfigs = new List<CommissionRangeConfigs>
    {
        new()
        {
            RangeStart = 0,
            RangeEnd = 1000,
            Type = CommissionType.Percentage,
            CommissionAmount = 0.05m,
            MinCommission = 10,   // At least $10
            MaxCommission = 50    // No more than $50 per tier
        },
        new()
        {
            RangeStart = 1000,
            RangeEnd = 0,
            Type = CommissionType.Percentage,
            CommissionAmount = 0.02m,
            MinCommission = 20,
            MaxCommission = 0     // Unlimited on second tier
        }
    }
};
```

### Selector-Based Example

```csharp
// Event ticketing: commission based on ticket count, applied to total price
var ticketRule = new CommissionRule
{
    CalculationType = CalculationType.Absolute, // Required for selector-based
    CommissionRangeConfigs = new List<CommissionRangeConfigs>
    {
        new() { RangeStart = 0, RangeEnd = 10, Type = CommissionType.Percentage, CommissionAmount = 0.1m },
        new() { RangeStart = 10, RangeEnd = 0, Type = CommissionType.Percentage, CommissionAmount = 0.05m }
    }
};

decimal orderTotal = 500m;
decimal ticketsSold = 15m;

// Uses second range (15 tickets), applies 5% to $500 order
decimal commission = Commission.ComputeCommission(orderTotal, ticketsSold, ticketRule);
// Result: $25
```

## Conventions

- **Infinity as 0**: Set `RangeEnd = 0` or `MaxCommission = 0` to represent ∞
- **Complete coverage**: Rules must cover [0, ∞) with no gaps
- **Exclusive end**: `RangeEnd` is exclusive (range is [Start, End))
- **Decimal precision**: Use `DecimalPlace` to control rounding (default: 4)

## Common Patterns

### Affiliate Marketing

```csharp
// Higher commission for higher sales
CalculationType = CalculationType.Absolute
Ranges:
  $0-$1000: 5%
  $1000-$5000: 7%
  $5000+: 10%
```

### Payment Processing

```csharp
// Tiered fees that accumulate
CalculationType = CalculationType.Proportional
Ranges:
  $0-$10,000: 2.9% + $0.30 min
  $10,000+: 1.5%
```

### Broker Fees

```csharp
// Fixed fee per transaction tier
CalculationType = CalculationType.Absolute
Ranges:
  $0-$10,000: $9.99
  $10,000-$100,000: $19.99
  $100,000+: $49.99
```

## License

MIT