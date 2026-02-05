namespace FemvedBackend.Api.Contracts.Guided;

/// <summary>
/// Represents a guided program for an expert.
/// </summary>
public sealed record GuidedExpertProgramDto(
    Guid ProgramId,
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<GuidedExpertProgramDurationDto> Durations);
