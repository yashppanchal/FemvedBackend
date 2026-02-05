namespace FemvedBackend.Api.Contracts.Guided;

/// <summary>
/// Represents a guided program in a flat list.
/// </summary>
public sealed record GuidedProgramFlatDto(
    Guid ProgramId,
    Guid CategoryId,
    Guid ExpertId,
    string? ExpertBio,
    string? ExpertSpecialization,
    decimal ExpertRating,
    bool ExpertIsVerified,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<GuidedDurationFlatDto> Durations);
