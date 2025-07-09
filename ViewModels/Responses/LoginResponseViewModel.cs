namespace EventOrganizationSystem.ViewModels.Responses;
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
    
    /// <summary>
    /// User's ID
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    public LoginResponseViewModel(int id, string token, string email, string role)
    {
        Id = id;
        Token = token;
        Email = email;
        Role = role;
    }
}