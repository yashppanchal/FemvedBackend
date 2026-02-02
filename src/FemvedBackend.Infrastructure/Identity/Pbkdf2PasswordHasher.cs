using System.Security.Cryptography;
using FemvedBackend.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FemvedBackend.Infrastructure.Identity;

public sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, KeySize);

        return string.Join('.', Iterations, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            return false;
        }

        var parts = hashedPassword.Split('.', 3, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
        {
            return false;
        }

        var saltBytes = Convert.FromBase64String(parts[1]);
        var expectedHashBytes = Convert.FromBase64String(parts[2]);
        var actualHashBytes = KeyDerivation.Pbkdf2(password, saltBytes, KeyDerivationPrf.HMACSHA256, iterations, KeySize);

        return CryptographicOperations.FixedTimeEquals(actualHashBytes, expectedHashBytes);
    }
}
