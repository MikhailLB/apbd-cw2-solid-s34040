namespace ConsoleApp3.Models;

public class Student : User
{
    public string StudentNumber { get; set; }

    public Student(string firstName, string lastName, string studentNumber)
        : base(firstName, lastName)
    {
        StudentNumber = studentNumber;
    }

    public override int MaxActiveRentals => 2;
    public override string UserType => "Student";
}
