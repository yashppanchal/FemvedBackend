namespace FemvedBackend.Api.Contracts.Profile;

/// <summary>
/// Represents the current user's profile response.
/// </summary>
public sealed record ProfileResponseDto(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string CountryCode,
    string MobileNumber,
    bool IsMobileVerified,
    bool IsEmailVerified,
    string Role);
