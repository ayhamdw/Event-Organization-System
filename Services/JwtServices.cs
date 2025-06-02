using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Event_Organization_System.IServices;
using Microsoft.IdentityModel.Tokens;

namespace Event_Organization_System.Services;

public class JwtServices : IJwtService
{
    public string GenerateToken(string email, string role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT__Key") ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: Environment.GetEnvironmentVariable("Jwt__Issuer"),
            audience: Environment.GetEnvironmentVariable("Jwt__Audience"),
            claims: claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}