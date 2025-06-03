using Event_Organization_System.IServices;
using Event_Organization_System.model;
using Event_Organization_System.ViewModels;
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

    public async Task<EventResponseViewModel> CreateEventAsync(EventViewModel eventViewModel)
    {
        ArgumentNullException.ThrowIfNull(eventViewModel);

        var newEvent = new Event
        {
            Title = eventViewModel.Title,
            Description = eventViewModel.Description,
            Time = eventViewModel.Time,
            Location = eventViewModel.Location,
            TotalSeats = eventViewModel.TotalSeats,
            RemainingSeats = eventViewModel.RemainingSeats,
            CreatedBy = eventViewModel.CreatedBy
        };
        
        await _context.Events.AddAsync(newEvent);
        await _context.SaveChangesAsync();
        
        var eventResponse = new EventResponseViewModel(newEvent);
        return eventResponse;
    }
}