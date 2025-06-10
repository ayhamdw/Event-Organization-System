namespace Event_Organization_System.ViewModels.Responses;
/// <summary>
/// Login response data
/// </summary>
public class LoginResponseViewModel
{
    /// <summary>
    /// JWT authentication token
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    public string Token { get; set; }
    /// <summary>
    /// User's email address
    /// </summary>
    /// <example>ayham.1395@gmail.com</example>
    public string Email { get; set; }
    /// <summary>
    /// User's role
    /// </summary>
    /// <example>User</example>
    public string Role { get; set; }

    public LoginResponseViewModel(string token, string email, string role)
    {
        Token = token;
        Email = email;
        Role = role;
    }
}