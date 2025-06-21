namespace EventOrganizationSystem.IServices;

public interface IJwtService
{
    string GenerateToken(int id, string role);
}