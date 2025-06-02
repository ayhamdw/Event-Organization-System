using Event_Organization_System.IServices;
using Event_Organization_System.model;
using Microsoft.EntityFrameworkCore;

namespace Event_Organization_System.Services;

public class UserServices : IUserServices
{
    private readonly ApplicationDbContext _context;
    
    public UserServices(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> IsUserExists(string email)
    {
        var isUserExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (isUserExists == null)
        {
            return false;
        }
        return true;
    }
}