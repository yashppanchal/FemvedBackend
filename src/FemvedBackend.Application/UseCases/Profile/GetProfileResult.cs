namespace FemvedBackend.Application.UseCases.Profile;

/// <summary>
/// Represents the current user's profile.
/// </summary>
public sealed record GetProfileResult(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string CountryCode,
    string MobileNumber,
    bool IsMobileVerified,
    bool IsEmailVerified,
    string Role);
