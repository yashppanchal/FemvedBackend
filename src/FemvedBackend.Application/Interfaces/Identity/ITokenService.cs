using FemvedBackend.Application.Identity;

namespace FemvedBackend.Application.Interfaces.Identity;

/// <summary>
/// Provides JWT and refresh token operations.
/// </summary>
public interface ITokenService
{
    Task<AuthTokenResult> GenerateTokensAsync(
        Guid userId,
        string email,
        IReadOnlyCollection<string> roles,
        string country,
        string currency,
        CancellationToken cancellationToken = default);

    Task<AuthTokenResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task RevokeAsync(string refreshToken, CancellationToken cancellationToken = default);
}
