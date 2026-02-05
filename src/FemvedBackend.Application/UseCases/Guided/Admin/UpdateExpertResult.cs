namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of updating an expert.
/// </summary>
public sealed record UpdateExpertResult(
    Guid UserId,
    string? Bio,
    string? Specialization,
    decimal Rating,
    bool IsVerified);
