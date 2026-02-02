namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents a request to start a password reset flow.
/// </summary>
public sealed record ForgotPasswordRequestDto(
    string Email,
    string ResetLinkBase);
