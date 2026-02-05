namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of updating program pricing.
/// </summary>
public sealed record UpdateProgramPricingResult(
    Guid Id,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
