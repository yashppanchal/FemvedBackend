using FemvedBackend.Application.Identity;

namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents the outcome of a sign-in attempt.
/// </summary>
public sealed record SignInResult(
    Guid UserId,
    string Email,
    AuthTokenResult Tokens);
