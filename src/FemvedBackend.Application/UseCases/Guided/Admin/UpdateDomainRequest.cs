namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to update a domain.
/// </summary>
public sealed record UpdateDomainRequest(
    short DomainId,
    string Name,
    string? Description,
    bool IsActive);
