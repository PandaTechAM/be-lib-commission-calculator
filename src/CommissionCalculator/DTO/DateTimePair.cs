namespace CommissionCalculator.DTO;

public class DateTimePair(DateTime startDate, DateTime endDate)
{
   public DateTime StartDate { get; } = startDate;
   public DateTime EndDate { get; } = endDate;
}