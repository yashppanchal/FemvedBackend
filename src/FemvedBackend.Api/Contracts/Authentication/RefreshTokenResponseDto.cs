namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents the response from a refresh token request.
/// </summary>
public sealed record RefreshTokenResponseDto(AuthTokenResponseDto Tokens);
