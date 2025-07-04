﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventOrganizationSystem.IServices;
using Microsoft.IdentityModel.Tokens;

namespace EventOrganizationSystem.Services;

public class JwtServices : IJwtService
{
    public string GenerateToken(int id, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier , Convert.ToString(id)),
            new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(id)),
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