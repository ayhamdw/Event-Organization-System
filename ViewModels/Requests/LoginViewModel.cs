using System.ComponentModel.DataAnnotations;

namespace Event_Organization_System.ViewModels;
/// <summary>
/// Login request model
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// User's email address
    /// </summary>
    /// <example>ayham.1396@gmail.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    /// <summary>
    /// User's password
    /// </summary>
    /// <example>1234Ayham**</example>
    [Required]
    public string Password { get; set; }
}