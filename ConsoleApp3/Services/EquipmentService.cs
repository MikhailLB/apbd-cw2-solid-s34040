namespace ConsoleApp3.Services;

using Models;
using Repositories;

public class EquipmentService
{
    private readonly EquipmentRepository _repository;

    public EquipmentService(EquipmentRepository repository)
    {
        _repository = repository;
    }

    public void Add(Equipment equipment) => _repository.Add(equipment);

    public Equipment? GetById(int id) => _repository.GetById(id);

    public List<Equipment> GetAll() => _repository.GetAll();

    public List<Equipment> GetAvailable() => _repository.GetAvailable();

    public OperationResult MarkAsUnavailable(int equipmentId)
    {
        var equipment = _repository.GetById(equipmentId);
        if (equipment == null)
            return OperationResult.Fail($"Equipment with ID {equipmentId} not found.");

        if (equipment.Status == EquipmentStatus.Borrowed)
            return OperationResult.Fail("Cannot mark borrowed equipment as unavailable. Return it first.");

        equipment.Status = EquipmentStatus.Unavailable;
        return OperationResult.Ok($"Equipment '{equipment.Name}' marked as unavailable.");
    }
}
