using EventOrganizationSystem.Enums;
using EventOrganizationSystem.IServices;
using EventOrganizationSystem.model;
using EventOrganizationSystem.ViewModels;
using EventOrganizationSystem.ViewModels.Responses;
using Microsoft.EntityFrameworkCore;

namespace EventOrganizationSystem.Services;

public class AuthServices: IAuthServices
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IUserServices _userServices;
    private readonly IValidatePasswordServices _validatePasswordServices;

    public AuthServices(ApplicationDbContext context , IJwtService jwtService , IUserServices userServices , IValidatePasswordServices validatePasswordServices)
    {
        _context = context;
        _jwtService = jwtService;
        _userServices = userServices;
        _validatePasswordServices = validatePasswordServices;
    }

    public async Task<LoginResponseViewModel> Login(LoginViewModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            throw new ArgumentException("Email and password are required");
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == model.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        
        var isSamePassword = _validatePasswordServices.IsSamePassword(model.Password, user.Password);
        if (!isSamePassword)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = _jwtService.GenerateToken(user.Id, user.Role.ToString());

        var result = new LoginResponseViewModel(user.Id ,token, user.Email, user.Role.ToString() , user.Name);
        return result;
    }

    public async Task<RegisterResponseViewModel> Register(RegisterViewModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Name))
        {
            throw new ArgumentException("Name, email, and password are required");
        }
        var existingUser = await _userServices.IsUserExists(model.Email);
        if (existingUser)
        {
            throw new InvalidOperationException("User with this email already exists");
        }
        
        var isValidPassword =  _validatePasswordServices.CheckIsValidPassword(model.Password);
        if (!isValidPassword)
        {
            throw new InvalidOperationException("Password does not meet the required criteria");
        }
        
        var hashedPassword = _validatePasswordServices.HashPassword(model.Password);
        var user = new User(model.Name, model.Email, hashedPassword, UserRole.User);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var token = _jwtService.GenerateToken(user.Id, user.Role.ToString());
        var result = new RegisterResponseViewModel(token, user.Email, user.Role.ToString());
        return result;
    }
}