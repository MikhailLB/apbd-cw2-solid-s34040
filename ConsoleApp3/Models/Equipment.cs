namespace ConsoleApp3.Models;

public abstract class Equipment
{
    private static int _nextId = 1;

    public int Id { get; }
    public string Name { get; set; }
    public EquipmentStatus Status { get; set; }

    protected Equipment(string name)
    {
        Id = _nextId++;
        Name = name;
        Status = EquipmentStatus.Available;
    }

    public abstract string GetDetails();
}
