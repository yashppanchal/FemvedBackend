namespace FemvedBackend.Api.Contracts.Guided;

/// <summary>
/// Represents guided program pricing by duration.
/// </summary>
public sealed record GuidedDurationDto(
    Guid DurationId,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
