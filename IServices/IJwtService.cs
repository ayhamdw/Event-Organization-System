namespace Event_Organization_System.IServices;

public interface IJwtService
{
    string GenerateToken(string email, string role);
}