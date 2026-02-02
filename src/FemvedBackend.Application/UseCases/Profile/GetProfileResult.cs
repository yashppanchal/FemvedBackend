namespace FemvedBackend.Application.UseCases.Profile;

/// <summary>
/// Represents the current user's profile.
/// </summary>
public sealed record GetProfileResult(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string Country,
    string Currency,
    string Role);
