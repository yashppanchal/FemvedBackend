namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents a request to log in.
/// </summary>
public sealed record LoginRequestDto(
    string Email,
    string Password);
