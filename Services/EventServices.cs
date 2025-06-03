using Event_Organization_System.IServices;
using Event_Organization_System.model;
using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

    public async Task<EventResponseViewModel> CreateEventAsync(EventViewModel eventViewModel , int userId)
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
            CreatedBy = userId
        };
        
        await _context.Events.AddAsync(newEvent);
        await _context.SaveChangesAsync();
        
        var eventResponse = new EventResponseViewModel(newEvent);
        return eventResponse;
    }

    public async Task<EventResponseViewModel> UpdateEventAsync(EventViewModel eventViewModel, int id , int userId)
    {
        if (eventViewModel == null || id <= 0)
        {
            throw new ArgumentException("Please provide a valid event view model and ID");
        }
        
        var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        
        if (existingEvent == null)
        {
            throw new KeyNotFoundException("Event not found");
        }

        if (userId != existingEvent.CreatedBy)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this event");
        }
        
        existingEvent.Title = eventViewModel.Title;
        existingEvent.Description = eventViewModel.Description;
        existingEvent.Time = eventViewModel.Time;
        existingEvent.Location = eventViewModel.Location;
        existingEvent.TotalSeats = eventViewModel.TotalSeats;
        existingEvent.RemainingSeats = eventViewModel.RemainingSeats;
        
        await _context.SaveChangesAsync();
        var eventResponse = new EventResponseViewModel(existingEvent);
        return eventResponse;
    }
}