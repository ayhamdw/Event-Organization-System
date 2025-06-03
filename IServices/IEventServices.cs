using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;

namespace Event_Organization_System.IServices;

public interface IEventServices
{
    Task<List<EventResponseViewModel>> GetAllEventsAsync();
    Task<EventResponseViewModel> CreateEventAsync(EventViewModel eventViewModel);
}