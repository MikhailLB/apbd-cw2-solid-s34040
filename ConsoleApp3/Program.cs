using ConsoleApp3.Repositories;
using ConsoleApp3.Services;
using ConsoleApp3.UI;

var equipmentRepo = new EquipmentRepository();
var userRepo = new UserRepository();
var rentalRepo = new RentalRepository();

IPenaltyCalculator penaltyCalculator = new PenaltyCalculator();

var equipmentService = new EquipmentService(equipmentRepo);
var userService = new UserService(userRepo);
var rentalService = new RentalService(rentalRepo, equipmentRepo, userRepo, penaltyCalculator);
var reportService = new ReportService(equipmentRepo, userRepo, rentalRepo);

var menu = new ConsoleMenu(equipmentService, userService, rentalService, reportService);

Console.WriteLine("Select mode:");
Console.WriteLine("  1. Run demo scenario");
Console.WriteLine("  2. Interactive menu");
Console.Write("Choice: ");

var choice = Console.ReadLine();
Console.WriteLine();

if (choice == "1")
    menu.RunDemo();
else
    menu.RunInteractive();
