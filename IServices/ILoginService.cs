using Event_Organization_System.ViewModels;

namespace Event_Organization_System.IServices;

public interface ILoginService
{
    bool Login(LoginViewModel model);
}