namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin request to create or update program pricing.
/// </summary>
public sealed record AdminProgramPricingRequestDto(
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
