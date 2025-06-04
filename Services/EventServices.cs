using Event_Organization_System.IServices;
using Event_Organization_System.model;
using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Event_Organization_System.Generic;

namespace Event_Organization_System.Services;

public class EventServices : IEventServices
{
    private readonly ApplicationDbContext _context;

    public EventServices(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<List<EventResponseViewModel>> GetAllEventsAsync(GetEventViewModel getEventViewModel)
    {
        var events =  await _context.Events.ToListAsync();
        var newEvents = GeneralPaginationAndFilter.ApplyPagination(events, getEventViewModel.PageNumber,
            getEventViewModel.PageSize);
        var eventResponses = newEvents.Select(e => new EventResponseViewModel(e)).ToList();
        return eventResponses;
    }

    public async Task<EventResponseViewModel> GetEventByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Please provide a valid event id");
        }

        var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        if (existingEvent == null)
        {
            throw new KeyNotFoundException("Event not found");
        }
        var eventResponse = new EventResponseViewModel(existingEvent);
        return eventResponse;
    }

    public async Task<EventResponseViewModel> CreateEventAsync(CreateEventViewModel createEventViewModel , int userId)
    {
        ArgumentNullException.ThrowIfNull(createEventViewModel);

        var newEvent = new Event
        {
            Title = createEventViewModel.Title,
            Description = createEventViewModel.Description,
            Time = createEventViewModel.Time,
            Location = createEventViewModel.Location,
            TotalSeats = createEventViewModel.TotalSeats,
            RemainingSeats = createEventViewModel.RemainingSeats,
            CreatedBy = userId
        };
        
        await _context.Events.AddAsync(newEvent);
        await _context.SaveChangesAsync();
        
        var eventResponse = new EventResponseViewModel(newEvent);
        return eventResponse;
    }

    public async Task<EventResponseViewModel> UpdateEventAsync(CreateEventViewModel createEventViewModel, int id , int userId)
    {
        if (createEventViewModel == null || id <= 0)
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
        
        existingEvent.Title = createEventViewModel.Title;
        existingEvent.Description = createEventViewModel.Description;
        existingEvent.Time = createEventViewModel.Time;
        existingEvent.Location = createEventViewModel.Location;
        existingEvent.TotalSeats = createEventViewModel.TotalSeats;
        existingEvent.RemainingSeats = createEventViewModel.RemainingSeats;
        
        await _context.SaveChangesAsync();
        var eventResponse = new EventResponseViewModel(existingEvent);
        return eventResponse;
    }

    public async Task<bool> DeleteEventAsync(int id, int userId)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Please provide a valid event id");
        }
        
        var deletedEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        if (deletedEvent == null)
        {
            throw new KeyNotFoundException("Event not found");  
        }

        if (deletedEvent.CreatedBy != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this event");
        }

        _context.Events.Remove(deletedEvent);
        await _context.SaveChangesAsync();

        return true;
    }
}