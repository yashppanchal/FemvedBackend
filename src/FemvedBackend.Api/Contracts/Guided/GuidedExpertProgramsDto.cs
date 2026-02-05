namespace FemvedBackend.Api.Contracts.Guided;

/// <summary>
/// Represents an expert with guided programs.
/// </summary>
public sealed record GuidedExpertProgramsDto(
    Guid ExpertId,
    string? Bio,
    string? Specialization,
    decimal Rating,
    bool IsVerified,
    IReadOnlyList<GuidedExpertProgramDto> Programs);
