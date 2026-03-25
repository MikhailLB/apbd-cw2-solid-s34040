namespace ConsoleApp3.Services;

public interface IPenaltyCalculator
{
    decimal Calculate(DateTime dueDate, DateTime returnDate);
}
