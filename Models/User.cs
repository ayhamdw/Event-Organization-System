using Event_Organization_System.ViewModels;
using System.Security.Claims;
using Event_Organization_System.Enums;

namespace Event_Organization_System.model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }

    public User(string name, string email, string password, UserRole role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    } 
}