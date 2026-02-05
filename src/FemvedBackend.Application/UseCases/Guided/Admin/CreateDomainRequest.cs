namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to create a domain.
/// </summary>
public sealed record CreateDomainRequest(
    string Name,
    string? Description,
    bool IsActive);
