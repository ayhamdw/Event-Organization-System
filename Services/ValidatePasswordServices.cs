using System.Security.Cryptography;
using System.Text.RegularExpressions;
using EventOrganizationSystem.IServices;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Org.BouncyCastle.Crypto.Generators;

namespace EventOrganizationSystem.Services;

public class ValidatePasswordServices : IValidatePasswordServices
{
    public bool IsSamePassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split(".");
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid hashed password format");
        }

        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        var check = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 20000,
            numBytesRequested: 256 / 8
        );
        return hash.SequenceEqual(check);
    }

    public bool CheckIsValidPassword(string password)
    {
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMiniMaxChars = new Regex(@".{8,}");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty");
        }

        if (!hasNumber.IsMatch(password))
        {
            throw new ArgumentException("Password must contain at least one number");
        }

        if (!hasUpperChar.IsMatch(password))
        {
            throw new ArgumentException("Password must contain at least one upper case letter");
        }
        if (!hasMiniMaxChars.IsMatch(password))
        {
            throw new ArgumentException("Password must be at least 8 characters long");
        }
        
        if (!hasLowerChar.IsMatch(password))
        {
            throw new ArgumentException("Password must contain at least one lower case letter");
        }
        
        if (!hasSymbols.IsMatch(password))
        {
            throw new ArgumentException("Password must contain at least one special character");
        }
        
        return true;
    }

    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty");
        }
        byte [] salt = new byte [128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 20000,
            numBytesRequested: 256 / 8
        ));
        
        var finalPassword = $"{Convert.ToBase64String(salt)}.{hashedPassword}";
        return finalPassword;
    }
    
    
}