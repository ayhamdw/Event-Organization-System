using System.ComponentModel.DataAnnotations;

namespace Event_Organization_System.ViewModels;

/// <summary>
/// User registration request model
/// </summary>
public class RegisterViewModel
{
    /// <summary>
    /// User's full name
    /// </summary>
    /// <example>Ayham</example>
    [Required (ErrorMessage = "Name is required")]
    public string Name { get; set; }
    /// <summary>
    /// User's email address (must be unique)
    /// </summary>
    /// <example>ayham.1395@gmail.com</example>
    [Required]
    [EmailAddress (ErrorMessage = "Invalid email address")] 
    public string Email { get; set; }
    /// <summary>
    /// User's password (minimum 8 characters, must contain uppercase, lowercase, number, and special character)
    /// </summary>
    /// <example>1234Ayham**</example>
    [Required (ErrorMessage = "Password is required")]
    public string Password { get; set; }
}