namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents the response from a login request.
/// </summary>
public sealed record LoginResponseDto(
    string Token,
    DateTime ExpiresAt,
    LoginUserDto User);

/// <summary>
/// Represents the authenticated user.
/// </summary>
public sealed record LoginUserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string MobileNumber,
    bool IsEmailVerified,
    bool IsMobileVerified,
    LoginRoleDto Role);

/// <summary>
/// Represents the user's role.
/// </summary>
public sealed record LoginRoleDto(
    short Id,
    string Name);
