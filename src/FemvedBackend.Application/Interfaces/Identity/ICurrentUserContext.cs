namespace FemvedBackend.Application.Interfaces.Identity;

/// <summary>
/// Provides information about the current authenticated user.
/// </summary>
public interface ICurrentUserContext
{
    Guid? UserId { get; }
    string? Email { get; }
    IReadOnlyCollection<string> Roles { get; }
    bool IsAuthenticated { get; }
}
