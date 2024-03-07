namespace CommissionCalculator.DTO;

public class CommissionRangeConfigs
{
    public decimal RangeStart { get; set; }
    public decimal RangeEnd { get; set; } //0 means infinity
    public CommissionType Type { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal MinCommission { get; set; }
    public decimal MaxCommission { get; set; } //0 means infinity
}