namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to update an expert.
/// </summary>
public sealed record UpdateExpertRequest(
    Guid UserId,
    string? Bio,
    string? Specialization,
    decimal Rating,
    bool IsVerified);
