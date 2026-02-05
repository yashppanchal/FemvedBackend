namespace FemvedBackend.Api.Contracts.Guided;

/// <summary>
/// Represents guided pricing for an expert program.
/// </summary>
public sealed record GuidedExpertProgramDurationDto(
    Guid PricingId,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
