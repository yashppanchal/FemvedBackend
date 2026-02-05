namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin expert response.
/// </summary>
public sealed record AdminExpertResponseDto(
    Guid UserId,
    string? Bio,
    string? Specialization,
    decimal Rating,
    bool IsVerified);
