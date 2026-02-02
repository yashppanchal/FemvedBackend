using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Api.Contracts.Authentication;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
public sealed record RegisterRequestDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Country,
    string Currency,
    RoleType RoleType = RoleType.User);
