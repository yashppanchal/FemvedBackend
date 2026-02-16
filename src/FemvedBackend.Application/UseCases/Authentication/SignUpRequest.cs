namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
public sealed record SignUpRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string CountryCode,
    string MobileNumber);
