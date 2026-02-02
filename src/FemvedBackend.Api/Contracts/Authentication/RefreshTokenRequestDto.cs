namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents a request to refresh an access token.
/// </summary>
public sealed record RefreshTokenRequestDto(string RefreshToken);
