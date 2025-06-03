using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Organization_System.model;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Time { get; set; }
    public string Location { get; set; }
    public int TotalSeats { get; set; }
    public int RemainingSeats { get; set; }
    public int CreatedBy { get; set; }
    [ForeignKey("CreatedBy")]
    public User User { get; set; }
}
