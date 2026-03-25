namespace ConsoleApp3.UI;

using Models;
using Services;

public class ConsoleMenu
{
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly RentalService _rentalService;
    private readonly ReportService _reportService;

    public ConsoleMenu(
        EquipmentService equipmentService,
        UserService userService,
        RentalService rentalService,
        ReportService reportService)
    {
        _equipmentService = equipmentService;
        _userService = userService;
        _rentalService = rentalService;
        _reportService = reportService;
    }

    public void RunDemo()
    {
        Console.WriteLine("=== DEMO SCENARIO ===\n");

        Console.WriteLine("--- Adding equipment ---");
        var laptop1 = new Laptop("Dell XPS 15", 16, "Intel i7-12700H");
        var laptop2 = new Laptop("MacBook Pro 14", 32, "Apple M3 Pro");
        var projector1 = new Projector("Epson EB-W51", 4000, "1920x1080");
        var camera1 = new Camera("Canon EOS R6", 20.1, "RF 24-105mm");
        var camera2 = new Camera("Sony A7 IV", 33.0, "FE 28-70mm");

        _equipmentService.Add(laptop1);
        _equipmentService.Add(laptop2);
        _equipmentService.Add(projector1);
        _equipmentService.Add(camera1);
        _equipmentService.Add(camera2);

        foreach (var eq in _equipmentService.GetAll())
            Console.WriteLine($"  Added: {eq.GetDetails()}");

        Console.WriteLine("\n--- Adding users ---");
        var student = new Student("Jan", "Kowalski", "s12345");
        var employee = new Employee("Anna", "Nowak", "IT");

        _userService.Add(student);
        _userService.Add(employee);

        foreach (var u in _userService.GetAll())
            Console.WriteLine($"  Added: [{u.UserType}] {u.FullName} (ID: {u.Id})");

        Console.WriteLine("\n--- Borrowing equipment ---");
        PrintResult(_rentalService.Borrow(student.Id, laptop1.Id));
        PrintResult(_rentalService.Borrow(student.Id, camera1.Id));
        PrintResult(_rentalService.Borrow(employee.Id, projector1.Id, DateTime.Now.AddDays(-30)));

        Console.WriteLine("\n--- Attempting invalid operations ---");
        PrintResult(_rentalService.Borrow(student.Id, camera2.Id));

        var markResult = _equipmentService.MarkAsUnavailable(laptop2.Id);
        PrintResult(markResult);
        PrintResult(_rentalService.Borrow(employee.Id, laptop2.Id));

        Console.WriteLine("\n--- Returning equipment on time ---");
        PrintResult(_rentalService.Return(camera1.Id));

        Console.WriteLine("\n--- Simulating late return ---");
        PrintResult(_rentalService.Borrow(student.Id, camera2.Id, DateTime.Now.AddDays(-20)));
        PrintResult(_rentalService.Return(camera2.Id));

        Console.WriteLine("\n--- Active rentals for Jan Kowalski ---");
        var studentRentals = _rentalService.GetActiveByUser(student.Id);
        if (studentRentals.Count == 0)
            Console.WriteLine("  No active rentals.");
        foreach (var r in studentRentals)
        {
            var eq = _equipmentService.GetById(r.EquipmentId);
            Console.WriteLine($"  Rental #{r.Id}: {eq?.Name} | Due: {r.DueDate:yyyy-MM-dd}");
        }

        Console.WriteLine("\n--- Overdue rentals ---");
        var overdue = _rentalService.GetOverdueRentals();
        if (overdue.Count == 0)
            Console.WriteLine("  No overdue rentals.");
        foreach (var r in overdue)
        {
            var eq = _equipmentService.GetById(r.EquipmentId);
            var user = _userService.GetById(r.UserId);
            Console.WriteLine($"  Rental #{r.Id}: {eq?.Name} by {user?.FullName} | Due: {r.DueDate:yyyy-MM-dd}");
        }

        Console.WriteLine("\n--- System Report ---");
        Console.Write(_reportService.GenerateSummary());
    }

    public void RunInteractive()
    {
        while (true)
        {
            Console.WriteLine("\n========== EQUIPMENT RENTAL SYSTEM ==========");
            Console.WriteLine(" 1.  Add user");
            Console.WriteLine(" 2.  Add equipment");
            Console.WriteLine(" 3.  List all equipment");
            Console.WriteLine(" 4.  List available equipment");
            Console.WriteLine(" 5.  Borrow equipment");
            Console.WriteLine(" 6.  Return equipment");
            Console.WriteLine(" 7.  Mark equipment as unavailable");
            Console.WriteLine(" 8.  Show user's active rentals");
            Console.WriteLine(" 9.  Show overdue rentals");
            Console.WriteLine("10.  Generate report");
            Console.WriteLine(" 0.  Exit");
            Console.Write("\nChoice: ");

            var input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1": AddUser(); break;
                case "2": AddEquipment(); break;
                case "3": ListAllEquipment(); break;
                case "4": ListAvailableEquipment(); break;
                case "5": BorrowEquipment(); break;
                case "6": ReturnEquipment(); break;
                case "7": MarkUnavailable(); break;
                case "8": ShowUserRentals(); break;
                case "9": ShowOverdue(); break;
                case "10": ShowReport(); break;
                case "0": return;
                default: Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private void AddUser()
    {
        Console.Write("Type (1 - Student, 2 - Employee): ");
        var type = Console.ReadLine();
        Console.Write("First name: ");
        var firstName = Console.ReadLine() ?? "";
        Console.Write("Last name: ");
        var lastName = Console.ReadLine() ?? "";

        if (type == "1")
        {
            Console.Write("Student number: ");
            var studentNumber = Console.ReadLine() ?? "";
            var user = new Student(firstName, lastName, studentNumber);
            _userService.Add(user);
            Console.WriteLine($"Student added with ID {user.Id}.");
        }
        else if (type == "2")
        {
            Console.Write("Department: ");
            var department = Console.ReadLine() ?? "";
            var user = new Employee(firstName, lastName, department);
            _userService.Add(user);
            Console.WriteLine($"Employee added with ID {user.Id}.");
        }
        else
        {
            Console.WriteLine("Invalid user type.");
        }
    }

    private void AddEquipment()
    {
        Console.Write("Type (1 - Laptop, 2 - Projector, 3 - Camera): ");
        var type = Console.ReadLine();
        Console.Write("Name: ");
        var name = Console.ReadLine() ?? "";

        switch (type)
        {
            case "1":
                Console.Write("RAM (GB): ");
                if (!int.TryParse(Console.ReadLine(), out var ram))
                {
                    Console.WriteLine("Invalid value.");
                    return;
                }
                Console.Write("Processor: ");
                var cpu = Console.ReadLine() ?? "";
                var laptop = new Laptop(name, ram, cpu);
                _equipmentService.Add(laptop);
                Console.WriteLine($"Laptop added with ID {laptop.Id}.");
                break;
            case "2":
                Console.Write("Lumens: ");
                if (!int.TryParse(Console.ReadLine(), out var lumens))
                {
                    Console.WriteLine("Invalid value.");
                    return;
                }
                Console.Write("Resolution: ");
                var res = Console.ReadLine() ?? "";
                var proj = new Projector(name, lumens, res);
                _equipmentService.Add(proj);
                Console.WriteLine($"Projector added with ID {proj.Id}.");
                break;
            case "3":
                Console.Write("Megapixels: ");
                if (!double.TryParse(Console.ReadLine(), out var mp))
                {
                    Console.WriteLine("Invalid value.");
                    return;
                }
                Console.Write("Lens type: ");
                var lens = Console.ReadLine() ?? "";
                var cam = new Camera(name, mp, lens);
                _equipmentService.Add(cam);
                Console.WriteLine($"Camera added with ID {cam.Id}.");
                break;
            default:
                Console.WriteLine("Invalid equipment type.");
                break;
        }
    }

    private void ListAllEquipment()
    {
        var all = _equipmentService.GetAll();
        if (all.Count == 0)
        {
            Console.WriteLine("No equipment in system.");
            return;
        }
        foreach (var eq in all)
            Console.WriteLine($"  [{eq.Id}] {eq.GetDetails()}");
    }

    private void ListAvailableEquipment()
    {
        var available = _equipmentService.GetAvailable();
        if (available.Count == 0)
        {
            Console.WriteLine("No available equipment.");
            return;
        }
        foreach (var eq in available)
            Console.WriteLine($"  [{eq.Id}] {eq.GetDetails()}");
    }

    private void BorrowEquipment()
    {
        Console.Write("User ID: ");
        if (!int.TryParse(Console.ReadLine(), out var userId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }
        Console.Write("Equipment ID: ");
        if (!int.TryParse(Console.ReadLine(), out var eqId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }
        PrintResult(_rentalService.Borrow(userId, eqId));
    }

    private void ReturnEquipment()
    {
        Console.Write("Equipment ID: ");
        if (!int.TryParse(Console.ReadLine(), out var eqId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }
        PrintResult(_rentalService.Return(eqId));
    }

    private void MarkUnavailable()
    {
        Console.Write("Equipment ID: ");
        if (!int.TryParse(Console.ReadLine(), out var eqId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }
        PrintResult(_equipmentService.MarkAsUnavailable(eqId));
    }

    private void ShowUserRentals()
    {
        Console.Write("User ID: ");
        if (!int.TryParse(Console.ReadLine(), out var userId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }
        var rentals = _rentalService.GetActiveByUser(userId);
        if (rentals.Count == 0)
        {
            Console.WriteLine("No active rentals.");
            return;
        }
        foreach (var r in rentals)
        {
            var eq = _equipmentService.GetById(r.EquipmentId);
            Console.WriteLine($"  Rental #{r.Id}: {eq?.Name} | Due: {r.DueDate:yyyy-MM-dd}");
        }
    }

    private void ShowOverdue()
    {
        var overdue = _rentalService.GetOverdueRentals();
        if (overdue.Count == 0)
        {
            Console.WriteLine("No overdue rentals.");
            return;
        }
        foreach (var r in overdue)
        {
            var eq = _equipmentService.GetById(r.EquipmentId);
            var user = _userService.GetById(r.UserId);
            Console.WriteLine($"  Rental #{r.Id}: {eq?.Name} by {user?.FullName} | Due: {r.DueDate:yyyy-MM-dd}");
        }
    }

    private void ShowReport()
    {
        Console.Write(_reportService.GenerateSummary());
    }

    private static void PrintResult(OperationResult result)
    {
        var prefix = result.Success ? "[OK]" : "[FAIL]";
        Console.WriteLine($"  {prefix} {result.Message}");
    }
}
