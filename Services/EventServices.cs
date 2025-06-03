using Event_Organization_System.IServices;
using Event_Organization_System.model;
using Event_Organization_System.ViewModels.Responses;
using Microsoft.EntityFrameworkCore;

namespace Event_Organization_System.Services;

public class EventServices : IEventServices
{
    private readonly ApplicationDbContext _context;

    public EventServices(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<List<EventResponseViewModel>> GetAllEventsAsync()
    {
        var events = await _context.Events.ToListAsync();
        var eventResponses = events.Select(e => new EventResponseViewModel(e)).ToList();
        return eventResponses;
    }
}