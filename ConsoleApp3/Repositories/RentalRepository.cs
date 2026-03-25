namespace ConsoleApp3.Repositories;

using Models;

public class RentalRepository
{
    private readonly List<Rental> _rentals = new();

    public void Add(Rental rental) => _rentals.Add(rental);

    public List<Rental> GetAll() => _rentals.ToList();

    public List<Rental> GetActiveByUserId(int userId) =>
        _rentals.Where(r => r.UserId == userId && r.IsActive).ToList();

    public Rental? GetActiveByEquipmentId(int equipmentId) =>
        _rentals.FirstOrDefault(r => r.EquipmentId == equipmentId && r.IsActive);

    public List<Rental> GetOverdue() =>
        _rentals.Where(r => r.IsOverdue).ToList();
}
