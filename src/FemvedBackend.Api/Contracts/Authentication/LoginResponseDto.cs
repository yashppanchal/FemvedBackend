namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents the response from a login request.
/// </summary>
public sealed record LoginResponseDto(
    Guid UserId,
    string Email,
    AuthTokenResponseDto Tokens);
