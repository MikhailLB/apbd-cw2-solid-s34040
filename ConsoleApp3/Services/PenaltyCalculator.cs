namespace ConsoleApp3.Services;

public class PenaltyCalculator : IPenaltyCalculator
{
    private const decimal PenaltyPerDay = 10.00m;

    public decimal Calculate(DateTime dueDate, DateTime returnDate)
    {
        if (returnDate <= dueDate)
            return 0;

        var daysLate = (int)(returnDate - dueDate).TotalDays;
        return daysLate * PenaltyPerDay;
    }
}
