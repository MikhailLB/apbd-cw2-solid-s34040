namespace ConsoleApp3.Repositories;

using Models;

public class EquipmentRepository
{
    private readonly List<Equipment> _items = new();

    public void Add(Equipment equipment) => _items.Add(equipment);

    public Equipment? GetById(int id) => _items.FirstOrDefault(e => e.Id == id);

    public List<Equipment> GetAll() => _items.ToList();

    public List<Equipment> GetAvailable() =>
        _items.Where(e => e.Status == EquipmentStatus.Available).ToList();
}
