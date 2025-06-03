using Event_Organization_System.model;

namespace Event_Organization_System.ViewModels.Responses;

public class EventResponseViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Time  { get; set; }
    public string Location  { get; set; }
    public int RemainingSeats { get; set; }
    
    public EventResponseViewModel(Event eventEntity)
    {
        Title = eventEntity.Title;
        Description = eventEntity.Description;
        Time = eventEntity.Time;
        Location = eventEntity.Location;
        RemainingSeats = eventEntity.RemainingSeats;
    }
}