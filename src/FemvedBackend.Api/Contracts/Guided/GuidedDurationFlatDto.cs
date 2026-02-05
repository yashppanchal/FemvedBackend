namespace FemvedBackend.Api.Contracts.Guided;

/// <summary>
/// Represents guided program pricing in a flat list.
/// </summary>
public sealed record GuidedDurationFlatDto(
    Guid PricingId,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
