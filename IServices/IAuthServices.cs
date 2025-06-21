using EventOrganizationSystem.ViewModels;
using EventOrganizationSystem.ViewModels.Responses;

namespace EventOrganizationSystem.IServices;

public interface IAuthServices
{
    Task<LoginResponseViewModel> Login(LoginViewModel model);
    Task<RegisterResponseViewModel> Register(RegisterViewModel model);
}