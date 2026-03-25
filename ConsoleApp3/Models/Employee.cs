namespace ConsoleApp3.Models;

public class Employee : User
{
    public string Department { get; set; }

    public Employee(string firstName, string lastName, string department)
        : base(firstName, lastName)
    {
        Department = department;
    }

    public override int MaxActiveRentals => 5;
    public override string UserType => "Employee";
}
