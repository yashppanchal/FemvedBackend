namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents a request to reset a password.
/// </summary>
public sealed record ResetPasswordRequestDto(
    string Email,
    string ResetToken,
    string NewPassword);
