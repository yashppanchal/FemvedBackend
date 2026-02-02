namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents the response from a registration request.
/// </summary>
public sealed record RegisterResponseDto(
    Guid UserId,
    string Email,
    AuthTokenResponseDto Tokens);
