namespace Event_Organization_System.ViewModels.Responses;

public class LoginResponseViewModel
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public LoginResponseViewModel(string token, string email, string role)
    {
        Token = token;
        Email = email;
        Role = role;
    }
}