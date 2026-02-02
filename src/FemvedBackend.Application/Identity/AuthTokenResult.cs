namespace FemvedBackend.Application.Identity;

/// <summary>
/// Represents generated access and refresh tokens.
/// </summary>
public sealed record AuthTokenResult(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);
