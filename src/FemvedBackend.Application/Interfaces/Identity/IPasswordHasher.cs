namespace FemvedBackend.Application.Interfaces.Identity;

/// <summary>
/// Provides password hashing and verification.
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
