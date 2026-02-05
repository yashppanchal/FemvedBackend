namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to update program pricing.
/// </summary>
public sealed record UpdateProgramPricingRequest(
    Guid PricingId,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
