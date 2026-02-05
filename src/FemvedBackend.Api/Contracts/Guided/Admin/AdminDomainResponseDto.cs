namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin domain response.
/// </summary>
public sealed record AdminDomainResponseDto(
    short Id,
    string Name,
    string? Description,
    bool IsActive);
