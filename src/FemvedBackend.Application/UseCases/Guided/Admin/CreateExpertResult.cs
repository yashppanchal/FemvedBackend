namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of creating an expert.
/// </summary>
public sealed record CreateExpertResult(
    Guid UserId,
    string? Bio,
    string? Specialization,
    decimal Rating,
    bool IsVerified);
