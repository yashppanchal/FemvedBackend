namespace FemvedBackend.Api.Contracts.Experts;

/// <summary>
/// Represents program metadata for an expert.
/// </summary>
public sealed record ExpertProgramDto(
    Guid ProgramId,
    string Title,
    string? Description,
    string? Image,
    IReadOnlyList<ExpertProgramDurationDto> Durations);
