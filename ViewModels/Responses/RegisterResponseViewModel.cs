using Event_Organization_System.model;

namespace Event_Organization_System.ViewModels.Responses;

public class RegisterResponseViewModel
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    
    public RegisterResponseViewModel(string token, string email, string role)
    {
        Token = token;
        Email = email;
        Role = role;
    }
}