namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents authentication tokens returned by the API.
/// </summary>
public sealed record AuthTokenResponseDto(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);
