namespace FemvedBackend.Api.Contracts.Guided;

/// <summary>
/// Represents a guided program.
/// </summary>
public sealed record GuidedProgramDto(
    Guid ProgramId,
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<GuidedDurationDto> Durations);
