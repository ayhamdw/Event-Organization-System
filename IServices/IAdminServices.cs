using Event_Organization_System.ViewModels.Requests;
using Event_Organization_System.ViewModels.Responses;

namespace Event_Organization_System.IServices
{
    public interface IAdminServices
    {
        Task<List<UsersResponseViewModel>> GetAllUsersAsync(GetUsersViewModel getUsersViewModel);
    }
}