namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents a request to start a password reset flow.
/// </summary>
public sealed record ForgotPasswordRequest(
    string Email,
    string ResetLinkBase);
