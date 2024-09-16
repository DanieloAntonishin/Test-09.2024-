namespace Task.Models;

public class RentTime: Entity
{
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid? HallId { get; set; } = null;
}
