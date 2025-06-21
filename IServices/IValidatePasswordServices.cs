namespace EventOrganizationSystem.IServices;

public interface IValidatePasswordServices
{
    bool IsSamePassword(string password , string hashedPassword);
    bool CheckIsValidPassword(string password);
    
    string HashPassword(string password);
}