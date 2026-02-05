namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to create a program with pricing.
/// </summary>
public sealed record CreateProgramRequest(
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<CreateProgramPricingRequest> Pricing);

/// <summary>
/// Represents a request to create program pricing.
/// </summary>
public sealed record CreateProgramPricingRequest(
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
