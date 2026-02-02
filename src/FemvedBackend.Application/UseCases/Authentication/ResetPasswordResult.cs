namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents the outcome of a password reset.
/// </summary>
public sealed record ResetPasswordResult(
    Guid UserId,
    string Email);
