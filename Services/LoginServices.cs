using Event_Organization_System.IServices;
using Event_Organization_System.model;
using Event_Organization_System.ViewModels;

namespace Event_Organization_System.Services;

public class LoginServices: ILoginService
{
    private readonly ApplicationDbContext _context;

    public LoginServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool Login(LoginViewModel model)
    {
        var check = _context.Users.Any(u => u.Email == model.Email && u.Password == model.Password);
        return check;
    }
}