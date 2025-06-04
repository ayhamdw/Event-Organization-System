using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;

namespace Event_Organization_System.IServices;

public interface IEventServices
{
    Task<List<EventResponseViewModel>> GetAllEventsAsync(GetEventViewModel getEventViewModel);
    Task<EventResponseViewModel> GetEventByIdAsync(int id);
    Task<EventResponseViewModel> CreateEventAsync(CreateEventViewModel createEventViewModel, int userId);
    Task<EventResponseViewModel> UpdateEventAsync(CreateEventViewModel createEventViewModel , int id , int userId);
    Task<bool> DeleteEventAsync(int id, int userId);
}