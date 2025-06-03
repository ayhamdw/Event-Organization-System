namespace Event_Organization_System.IServices;

public interface IJwtService
{
    string GenerateToken(int id, string role);
}