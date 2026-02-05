namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of creating a program.
/// </summary>
public sealed record CreateProgramResult(
    Guid Id,
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<CreateProgramPricingResult> Pricing);

/// <summary>
/// Represents the result of creating program pricing.
/// </summary>
public sealed record CreateProgramPricingResult(
    Guid Id,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
