namespace FemvedBackend.Api.Contracts.Experts;

/// <summary>
/// Represents an expert response payload.
/// </summary>
public sealed record ExpertResponseDto(
    Guid Id,
    Guid UserId,
    string DisplayName,
    string? Bio);
