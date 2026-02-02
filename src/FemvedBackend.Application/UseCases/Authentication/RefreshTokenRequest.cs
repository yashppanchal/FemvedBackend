namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents a request to refresh authentication tokens.
/// </summary>
public sealed record RefreshTokenRequest(string RefreshToken);
