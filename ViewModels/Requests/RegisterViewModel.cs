using System.ComponentModel.DataAnnotations;

namespace Event_Organization_System.ViewModels;

public class RegisterViewModel
{
    [Required (ErrorMessage = "Name is required")]
    public string Name { get; set; }
    [Required]
    [EmailAddress (ErrorMessage = "Invalid email address")] 
    public string Email { get; set; }
    
    [Required (ErrorMessage = "Password is required")]
    public string Password { get; set; }
}