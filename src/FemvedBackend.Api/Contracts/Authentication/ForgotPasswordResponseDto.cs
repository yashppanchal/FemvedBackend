namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents the response from a forgot password request.
/// </summary>
public sealed record ForgotPasswordResponseDto(bool EmailSent);
