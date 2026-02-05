namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of updating a domain.
/// </summary>
public sealed record UpdateDomainResult(
    short Id,
    string Name,
    string? Description,
    bool IsActive);
