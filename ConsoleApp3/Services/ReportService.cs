namespace ConsoleApp3.Services;

using System.Text;
using Models;
using Repositories;

public class ReportService
{
    private readonly EquipmentRepository _equipmentRepository;
    private readonly UserRepository _userRepository;
    private readonly RentalRepository _rentalRepository;

    public ReportService(
        EquipmentRepository equipmentRepository,
        UserRepository userRepository,
        RentalRepository rentalRepository)
    {
        _equipmentRepository = equipmentRepository;
        _userRepository = userRepository;
        _rentalRepository = rentalRepository;
    }

    public string GenerateSummary()
    {
        var allEquipment = _equipmentRepository.GetAll();
        var allUsers = _userRepository.GetAll();
        var allRentals = _rentalRepository.GetAll();
        var activeRentals = allRentals.Where(r => r.IsActive).ToList();
        var overdueRentals = _rentalRepository.GetOverdue();
        var totalPenalties = allRentals.Sum(r => r.Penalty);

        var sb = new StringBuilder();
        sb.AppendLine("========== RENTAL SYSTEM REPORT ==========");
        sb.AppendLine($"  Total equipment:       {allEquipment.Count}");
        sb.AppendLine($"    Available:           {allEquipment.Count(e => e.Status == EquipmentStatus.Available)}");
        sb.AppendLine($"    Borrowed:            {allEquipment.Count(e => e.Status == EquipmentStatus.Borrowed)}");
        sb.AppendLine($"    Unavailable:         {allEquipment.Count(e => e.Status == EquipmentStatus.Unavailable)}");
        sb.AppendLine($"  Total users:           {allUsers.Count}");
        sb.AppendLine($"  Total rentals:         {allRentals.Count}");
        sb.AppendLine($"    Active:              {activeRentals.Count}");
        sb.AppendLine($"    Overdue:             {overdueRentals.Count}");
        sb.AppendLine($"  Total penalties:       {totalPenalties:C}");
        sb.AppendLine("===========================================");

        return sb.ToString();
    }
}
