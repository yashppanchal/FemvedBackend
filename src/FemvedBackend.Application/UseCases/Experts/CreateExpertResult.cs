namespace FemvedBackend.Application.UseCases.Experts;

/// <summary>
/// Represents the result of creating an expert.
/// </summary>
public sealed record CreateExpertResult(
    Guid ExpertId,
    Guid UserId,
    string DisplayName,
    string? Bio);
