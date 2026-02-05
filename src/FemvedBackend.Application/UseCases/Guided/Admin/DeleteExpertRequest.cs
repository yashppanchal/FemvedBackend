namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to delete an expert.
/// </summary>
public sealed record DeleteExpertRequest(Guid ExpertId);
