namespace CommissionCalculator.DTO;

public class CommissionRulesInfo
{
    public CalculationType CalculationType { get; set; }
    public List<CommissionRule> CommissionRules { get; set; }
}