namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to create program pricing for a program.
/// </summary>
public sealed record CreateProgramPricingEntryRequest(
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
