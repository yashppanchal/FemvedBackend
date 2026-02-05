namespace FemvedBackend.Application.UseCases.Guided;

/// <summary>
/// Represents an expert with guided programs and pricing.
/// </summary>
public sealed record GuidedExpertProgramsResult(
    Guid ExpertId,
    string? Bio,
    string? Specialization,
    decimal Rating,
    bool IsVerified,
    IReadOnlyList<GuidedExpertProgramResult> Programs);

/// <summary>
/// Represents a guided program for an expert.
/// </summary>
public sealed record GuidedExpertProgramResult(
    Guid ProgramId,
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<GuidedExpertProgramDurationResult> Durations);

/// <summary>
/// Represents guided pricing for an expert program.
/// </summary>
public sealed record GuidedExpertProgramDurationResult(
    Guid PricingId,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
