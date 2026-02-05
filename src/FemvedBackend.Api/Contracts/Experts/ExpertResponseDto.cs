namespace FemvedBackend.Api.Contracts.Experts;

/// <summary>
/// Represents an expert response payload.
/// </summary>
public sealed record ExpertResponseDto(
    Guid UserId,
    string? Bio,
    string? Specialization,
    decimal Rating,
    bool IsVerified);
