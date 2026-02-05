namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of creating a domain.
/// </summary>
public sealed record CreateDomainResult(
    short Id,
    string Name,
    string? Description,
    bool IsActive);
