namespace FemvedBackend.Application.Interfaces.Identity;

/// <summary>
/// Provides information about the current authenticated user for future flows.
/// </summary>
public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Email { get; }
    IReadOnlyCollection<string> Roles { get; }
    bool IsAuthenticated { get; }
}
