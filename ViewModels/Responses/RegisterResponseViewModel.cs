using EventOrganizationSystem.model;

namespace EventOrganizationSystem.ViewModels.Responses;
/// <summary>
/// Registration response data
/// </summary>
public class RegisterResponseViewModel
{
    /// <summary>
    /// JWT authentication token for the newly registered user
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    public string Token { get; set; }
    /// <summary>
    /// Registered user's email address
    /// </summary>
    /// <example>ayham.1395@gmail.com</example>
    public string Email { get; set; }
    /// <summary>
    /// Assigned user role
    /// </summary>
    /// <example>User</example>
    public string Role { get; set; }
    
    public RegisterResponseViewModel(string token, string email, string role)
    {
        Token = token;
        Email = email;
        Role = role;
    }
}