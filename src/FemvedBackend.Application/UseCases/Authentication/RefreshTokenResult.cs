using FemvedBackend.Application.Identity;

namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents the outcome of a token refresh operation.
/// </summary>
public sealed record RefreshTokenResult(AuthTokenResult Tokens);
