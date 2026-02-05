namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of creating program pricing.
/// </summary>
public sealed record CreateProgramPricingEntryResult(
    Guid Id,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
