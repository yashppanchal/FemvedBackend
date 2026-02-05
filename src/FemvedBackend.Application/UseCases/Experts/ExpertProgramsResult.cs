namespace FemvedBackend.Application.UseCases.Experts;

/// <summary>
/// Represents an expert with programs and duration-based pricing.
/// </summary>
public sealed record ExpertProgramsResult(
    Guid ExpertId,
    string ExpertName,
    string? Bio,
    IReadOnlyList<ExpertProgramResult> Programs);

/// <summary>
/// Represents program metadata for an expert.
/// </summary>
public sealed record ExpertProgramResult(
    Guid ProgramId,
    string Title,
    string? Description,
    string? ImageUrl,
    IReadOnlyList<ExpertProgramDurationResult> Durations);

/// <summary>
/// Represents duration-based pricing for a program.
/// </summary>
public sealed record ExpertProgramDurationResult(
    short Weeks,
    decimal Price,
    decimal? Discount,
    decimal? FinalPrice);
