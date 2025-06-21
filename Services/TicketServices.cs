using EventOrganizationSystem.Enums;
using EventOrganizationSystem.IServices;
using EventOrganizationSystem.model;
using EventOrganizationSystem.ViewModels;
using EventOrganizationSystem.ViewModels.Responses;
using Microsoft.EntityFrameworkCore;

namespace EventOrganizationSystem.Services;

public class TicketServices : ITicketServices
{
    
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TicketServices> _logger;
    private readonly IUserServices _userServices;

    public TicketServices(ApplicationDbContext context, ILogger<TicketServices> logger, IUserServices userServices)
    {
        _context = context;
        _logger = logger;
        _userServices = userServices;
    }

    public async Task<bool> BookSeatAsync(BookTicketViewModel bookTicketViewModel , int userId)
    {
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID: {UserId}", userId);
            throw new ArgumentException("User ID must be positive", nameof(userId));
        }

        if (bookTicketViewModel.EventId <= 0)
        {
            _logger.LogError("Invalid Event ID: {EventId}", bookTicketViewModel.EventId);
            throw new ArgumentException("Event ID must be positive", nameof(bookTicketViewModel.EventId));
        }

        if (bookTicketViewModel.Price < 0)
        {
            _logger.LogError("Negative Price: {Price}", bookTicketViewModel.Price);
            throw new ArgumentException("Price cannot be negative", nameof(bookTicketViewModel.Price));
        }

        if (!Enum.TryParse<TicketType>(bookTicketViewModel.TicketType , true, out var ticketType))
        {
            _logger.LogError("Invalid Ticket Type: {TicketType}", bookTicketViewModel.TicketType);
            throw new ArgumentException("Invalid ticket type", nameof(bookTicketViewModel.TicketType));
        }
        
        var isUserBookedTicket = await _userServices.IsUserBookedTicket(userId, bookTicketViewModel.EventId);
        if (isUserBookedTicket)
        {
            _logger.LogError("You have already booked a ticket for this event. User ID: {UserId}, Event ID: {EventId}", userId, bookTicketViewModel.EventId);
            throw new InvalidOperationException("You have already booked a ticket for this event.");
        }
        
        var ticket = new Ticket(userId
            , bookTicketViewModel.EventId ,
            ticketType,
            bookTicketViewModel.Price);
        
        var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Id == bookTicketViewModel.EventId);
        
        if (eventEntity == null)
        {
            _logger.LogError("Event not found for Event ID: {EventId}", bookTicketViewModel.EventId);
            throw new KeyNotFoundException("Event not found");
        }

        if (eventEntity.RemainingSeats < 1)
        {
            _logger.LogError("No remaining seats available for Event ID: {EventId}", bookTicketViewModel.EventId);
            throw new InvalidOperationException("No remaining seats available for this event");
        }

        eventEntity.RemainingSeats -= 1;
        await _context.Tickets.AddAsync(ticket);
        _context.Events.Update(eventEntity);
        
        await _context.SaveChangesAsync();
        _logger.LogInformation("Ticket booked successfully for User ID: {UserId}, Event ID: {EventId}", userId, bookTicketViewModel.EventId);
        return true;
    }

    public async Task<bool> CancelTicketAsync(int ticketId, int userId)
    {
        if (ticketId <= 0) 
        {
            _logger.LogError("Invalid Ticket ID: {TicketId}", ticketId);
            throw new ArgumentException("Ticket ID must be positive", nameof(ticketId));
        }

        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID: {UserId}", userId);
            throw new ArgumentException("User ID must be positive", nameof(userId));
        }
        
        var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId && t.UserId == userId);
        if (ticket == null) return false;
        
        var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Id == ticket.EventId);
        if (eventEntity == null)
        {
            _logger.LogError("Event not found for Event ID: {EventId}", ticket.EventId);
            throw new InvalidOperationException("Event not found");
        }

        if (eventEntity.Time.Date == DateTime.UtcNow.Date)
        {
            _logger.LogError("The ticket cannot be cancelled on the same day of the event.. Ticket ID: {TicketId}, User ID: {UserId}", ticketId, userId);
            throw new InvalidOperationException("The ticket cannot be cancelled on the same day of the event");
        }
        
        ticket.Status = TicketStatus.Cancelled;
        eventEntity.RemainingSeats += 1;

        _context.Tickets.Update(ticket);
        _context.Events.Update(eventEntity);
        
        await _context.SaveChangesAsync();
        _logger.LogInformation("Ticket cancelled successfully for Ticket ID: {TicketId}, User ID: {UserId}", ticketId, userId);
        
        return true;
    }

    public async Task<List<PersonalTicketResponseViewModel>> GetMyBookingAsync(int userId)
    {
        if (userId <= 0)
        {
            _logger.LogError("Invalid User ID: {UserId}", userId);
            throw new ArgumentException("User ID must be positive", nameof(userId));
        }

        var events = await _context.Tickets
            .Where(t => t.UserId == userId && t.Status != TicketStatus.Cancelled)
            .Join(_context.Events, t => t.EventId, e => e.Id, (t, e) => new PersonalTicketResponseViewModel(e.Title , e.Description , e.Location , e.Time))
            .ToListAsync();

        return events;

    }
}