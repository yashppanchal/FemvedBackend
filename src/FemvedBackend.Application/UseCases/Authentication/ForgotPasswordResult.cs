namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents the outcome of a forgot password request.
/// </summary>
public sealed record ForgotPasswordResult(bool EmailSent);
