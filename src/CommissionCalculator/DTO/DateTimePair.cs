namespace CommissionCalculator.DTO;

public class DateTimePair(DateTime startDate, DateTime endDate)
{
    public DateTime StartDate { get; set; } = startDate;
    public DateTime EndDate { get; set; } = endDate;
}