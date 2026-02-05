namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin request to create or update a domain.
/// </summary>
public sealed record AdminDomainRequestDto(
    string Name,
    string? Description,
    bool IsActive);
