using FemvedBackend.Application.Identity;

namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents the outcome of a sign-in attempt.
/// </summary>
public sealed record SignInResult(
    AuthTokenResult Tokens,
    SignInUserResult User);

/// <summary>
/// Represents the authenticated user.
/// </summary>
public sealed record SignInUserResult(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string MobileNumber,
    bool IsEmailVerified,
    bool IsMobileVerified,
    SignInRoleResult Role);

/// <summary>
/// Represents the user's role.
/// </summary>
public sealed record SignInRoleResult(
    short Id,
    string Name);
