using Event_Organization_System.Enums;
using Event_Organization_System.IServices;
using Event_Organization_System.model;
using Event_Organization_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Event_Organization_System.Services;

public class TicketServices : ITicketServices
{
    
    private readonly ApplicationDbContext _context;

    public TicketServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> BookSeatAsync(BookTicketViewModel bookTicketViewModel , int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be positive", nameof(userId));
        
        if (bookTicketViewModel.EventId <= 0)
            throw new ArgumentException("Event ID must be positive", nameof(bookTicketViewModel.EventId));
        
        if (bookTicketViewModel.Price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(bookTicketViewModel.Price));
        
        if (!Enum.IsDefined(typeof(TicketType), bookTicketViewModel.TicketType))
            throw new ArgumentException("Invalid ticket type", nameof(bookTicketViewModel.TicketType));
        
        var ticket = new Ticket(userId
            , bookTicketViewModel.EventId ,
            Enum.Parse<TicketType>(bookTicketViewModel.TicketType, true),
            bookTicketViewModel.Price);

        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelTicketAsync(int ticketId, int userId)
    {
        if (ticketId <= 0) throw new ArgumentException("Ticket ID must be positive", nameof(ticketId));
        if (userId <= 0) throw new ArgumentException("User ID must be positive", nameof(userId));

        var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId && t.UserId == userId);
        if (ticket == null) return false;

        ticket.Status = TicketStatus.Cancelled;

        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        
        return true;
    }
}