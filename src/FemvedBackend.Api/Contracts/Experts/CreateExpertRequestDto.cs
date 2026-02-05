namespace FemvedBackend.Api.Contracts.Experts;

/// <summary>
/// Represents a request to create an expert.
/// </summary>
public sealed record CreateExpertRequestDto(
    Guid UserId,
    string? Bio,
    string? Specialization);
