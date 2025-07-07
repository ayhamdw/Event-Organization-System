using EventOrganizationSystem.Enums;
using EventOrganizationSystem.IServices;
using EventOrganizationSystem.model;
using Microsoft.EntityFrameworkCore;

namespace EventOrganizationSystem.Services;

public class UserServices : IUserServices
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserServices> _logger;

    public UserServices(ApplicationDbContext context, ILogger<UserServices> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> IsUserExists(string email)
    {
        var isUserExists = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        return isUserExists != null;
    }

    public async Task<bool> IsUserBookedTicket(int userId, int eventId)
    {
        if (userId <= 0)
        {
            _logger.LogError("Invalid user id");
            throw new ArgumentException("User ID must be positive", nameof(userId));
        }

        if (eventId <= 0)
        {
            _logger.LogError("Invalid event id");
            throw new ArgumentException("Event ID must be positive", nameof(eventId));
        }
        
        var isUserBookedTicket = await _context.Tickets.AnyAsync(t => t.UserId == userId && t.EventId == eventId && t.Status != TicketStatus.Cancelled);
        return isUserBookedTicket;
    }
}