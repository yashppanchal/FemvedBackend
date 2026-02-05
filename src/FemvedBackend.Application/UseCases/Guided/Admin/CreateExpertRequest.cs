namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to create an expert.
/// </summary>
public sealed record CreateExpertRequest(
    Guid UserId,
    string? Bio,
    string? Specialization,
    decimal Rating,
    bool IsVerified);
