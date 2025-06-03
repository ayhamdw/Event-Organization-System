using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;

namespace Event_Organization_System.IServices;

public interface IEventServices
{
    Task<List<EventResponseViewModel>> GetAllEventsAsync();
    Task<EventResponseViewModel> CreateEventAsync(EventViewModel eventViewModel, int userId);
    Task<EventResponseViewModel> UpdateEventAsync(EventViewModel eventViewModel , int id , int userId);
    Task<bool> DeleteEventAsync(int id, int userId);
}