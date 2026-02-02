using FemvedBackend.Application.Identity;

namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents the outcome of a user registration.
/// </summary>
public sealed record SignUpResult(
    Guid UserId,
    string Email,
    AuthTokenResult Tokens);
