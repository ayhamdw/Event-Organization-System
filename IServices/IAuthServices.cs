using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;

namespace Event_Organization_System.IServices;

public interface IAuthServices
{
    Task<LoginResponseViewModel> Login(LoginViewModel model);
    Task<RegisterResponseViewModel> Register(RegisterViewModel model);
}