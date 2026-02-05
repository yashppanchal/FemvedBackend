namespace FemvedBackend.Application.UseCases.Guided;

/// <summary>
/// Represents a guided program with flat structure.
/// </summary>
public sealed record GuidedProgramFlatResult(
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
    IReadOnlyList<GuidedDurationFlatResult> Durations);

/// <summary>
/// Represents guided program pricing in a flat structure.
/// </summary>
public sealed record GuidedDurationFlatResult(
    Guid PricingId,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
