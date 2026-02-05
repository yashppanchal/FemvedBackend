namespace FemvedBackend.Application.UseCases.Guided;

/// <summary>
/// Represents guided program details with category, expert, and pricing.
/// </summary>
public sealed record GuidedProgramDetailsResult(
    Guid ProgramId,
    Guid CategoryId,
    GuidedCategoryDetailsResult Category,
    Guid ExpertId,
    GuidedExpertDetailsResult Expert,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<GuidedDurationDetailsResult> Durations);

/// <summary>
/// Represents guided category details.
/// </summary>
public sealed record GuidedCategoryDetailsResult(
    Guid CategoryId,
    short DomainId,
    Guid? ParentCategoryId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive);

/// <summary>
/// Represents guided expert details.
/// </summary>
public sealed record GuidedExpertDetailsResult(
    Guid ExpertId,
    string? Bio,
    string? Specialization,
    decimal Rating,
    bool IsVerified);

/// <summary>
/// Represents guided pricing details.
/// </summary>
public sealed record GuidedDurationDetailsResult(
    Guid PricingId,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
