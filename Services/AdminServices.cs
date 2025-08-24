using Event_Organization_System.IServices;
using Event_Organization_System.ViewModels.Requests;
using Event_Organization_System.ViewModels.Responses;
using EventOrganizationSystem.model;
using Microsoft.EntityFrameworkCore;

namespace Event_Organization_System.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly ApplicationDbContext _context;
        public AdminServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<UsersResponseViewModel>> GetAllUsersAsync(GetUsersViewModel getUsersViewModel)
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            var responseUsers = users.Select(u => new UsersResponseViewModel(u)).ToList();
            return responseUsers;
        }
    }
}