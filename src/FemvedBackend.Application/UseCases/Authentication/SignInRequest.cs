namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents a request to sign in.
/// </summary>
public sealed record SignInRequest(
    string Email,
    string Password);
