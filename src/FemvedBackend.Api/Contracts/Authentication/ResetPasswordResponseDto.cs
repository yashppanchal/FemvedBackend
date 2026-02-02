namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents the response from a password reset request.
/// </summary>
public sealed record ResetPasswordResponseDto(
    Guid UserId,
    string Email);
