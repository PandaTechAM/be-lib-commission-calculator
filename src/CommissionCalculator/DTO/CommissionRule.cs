namespace CommissionCalculator.DTO;

public class CommissionRule
{
   public CalculationType CalculationType { get; set; }
   public short DecimalPlace { get; set; } = 4;
   public List<CommissionRangeConfigs> CommissionRangeConfigs { get; set; } = null!;
}