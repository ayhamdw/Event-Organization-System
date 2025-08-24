using EventOrganizationSystem.Enums;
using EventOrganizationSystem.model;

namespace Event_Organization_System.ViewModels.Responses
{
    public class UsersResponseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }

        public UsersResponseViewModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Role = user.Role;
        }
    }
}