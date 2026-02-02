namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents a request to reset a password using a reset token.
/// </summary>
public sealed record ResetPasswordRequest(
    string Email,
    string ResetToken,
    string NewPassword);
