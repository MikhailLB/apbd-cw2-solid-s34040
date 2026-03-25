namespace ConsoleApp3.Services;

using Models;
using Repositories;

public class RentalService
{
    private const int DefaultRentalDays = 14;

    private readonly RentalRepository _rentalRepository;
    private readonly EquipmentRepository _equipmentRepository;
    private readonly UserRepository _userRepository;
    private readonly IPenaltyCalculator _penaltyCalculator;

    public RentalService(
        RentalRepository rentalRepository,
        EquipmentRepository equipmentRepository,
        UserRepository userRepository,
        IPenaltyCalculator penaltyCalculator)
    {
        _rentalRepository = rentalRepository;
        _equipmentRepository = equipmentRepository;
        _userRepository = userRepository;
        _penaltyCalculator = penaltyCalculator;
    }

    public OperationResult Borrow(int userId, int equipmentId, DateTime? borrowDate = null)
    {
        var user = _userRepository.GetById(userId);
        if (user == null)
            return OperationResult.Fail($"User with ID {userId} not found.");

        var equipment = _equipmentRepository.GetById(equipmentId);
        if (equipment == null)
            return OperationResult.Fail($"Equipment with ID {equipmentId} not found.");

        if (equipment.Status != EquipmentStatus.Available)
            return OperationResult.Fail($"Equipment '{equipment.Name}' is not available for borrowing.");

        var activeRentals = _rentalRepository.GetActiveByUserId(userId);
        if (activeRentals.Count >= user.MaxActiveRentals)
            return OperationResult.Fail(
                $"User '{user.FullName}' has reached the rental limit ({user.MaxActiveRentals}).");

        var startDate = borrowDate ?? DateTime.Now;
        var rental = new Rental(userId, equipmentId, startDate, startDate.AddDays(DefaultRentalDays));
        _rentalRepository.Add(rental);
        equipment.Status = EquipmentStatus.Borrowed;

        return OperationResult.Ok(
            $"Equipment '{equipment.Name}' borrowed by '{user.FullName}'. Due: {rental.DueDate:yyyy-MM-dd}");
    }

    public OperationResult Return(int equipmentId, DateTime? returnDate = null)
    {
        var rental = _rentalRepository.GetActiveByEquipmentId(equipmentId);
        if (rental == null)
            return OperationResult.Fail($"No active rental found for equipment ID {equipmentId}.");

        var equipment = _equipmentRepository.GetById(equipmentId);
        if (equipment == null)
            return OperationResult.Fail($"Equipment with ID {equipmentId} not found.");

        var actualReturn = returnDate ?? DateTime.Now;
        rental.ReturnedAt = actualReturn;
        rental.Penalty = _penaltyCalculator.Calculate(rental.DueDate, actualReturn);
        equipment.Status = EquipmentStatus.Available;

        if (rental.Penalty > 0)
            return OperationResult.Ok(
                $"Equipment '{equipment.Name}' returned late. Penalty: {rental.Penalty:C}");

        return OperationResult.Ok($"Equipment '{equipment.Name}' returned on time.");
    }

    public List<Rental> GetActiveByUser(int userId) =>
        _rentalRepository.GetActiveByUserId(userId);

    public List<Rental> GetOverdueRentals() =>
        _rentalRepository.GetOverdue();

    public List<Rental> GetAll() =>
        _rentalRepository.GetAll();
}
