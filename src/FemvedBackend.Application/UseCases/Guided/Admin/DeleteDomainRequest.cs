namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to delete a domain.
/// </summary>
public sealed record DeleteDomainRequest(short DomainId);
