namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin program pricing response.
/// </summary>
public sealed record AdminProgramPricingResponseDto(
    Guid Id,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
