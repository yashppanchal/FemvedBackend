namespace FemvedBackend.Application.UseCases.Experts;

/// <summary>
/// Represents a request to create an expert profile.
/// </summary>
public sealed record CreateExpertRequest(
    Guid UserId,
    string? Bio,
    string? Specialization);
