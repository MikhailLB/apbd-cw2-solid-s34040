namespace ConsoleApp3.Models;

public class Rental
{
    private static int _nextId = 1;

    public int Id { get; }
    public int UserId { get; }
    public int EquipmentId { get; }
    public DateTime RentedAt { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; set; }
    public decimal Penalty { get; set; }

    public bool IsActive => ReturnedAt == null;
    public bool IsOverdue => IsActive && DateTime.Now > DueDate;

    public Rental(int userId, int equipmentId, DateTime rentedAt, DateTime dueDate)
    {
        Id = _nextId++;
        UserId = userId;
        EquipmentId = equipmentId;
        RentedAt = rentedAt;
        DueDate = dueDate;
        Penalty = 0;
    }
}
