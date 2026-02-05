namespace FemvedBackend.Application.UseCases.Guided;

/// <summary>
/// Represents the guided domain tree.
/// </summary>
public sealed record GuidedDomainTreeResult(
    short DomainId,
    string Name,
    string? Description,
    bool IsActive,
    IReadOnlyList<GuidedCategoryTreeResult> Categories);

/// <summary>
/// Represents a guided category tree node.
/// </summary>
public sealed record GuidedCategoryTreeResult(
    Guid CategoryId,
    short DomainId,
    Guid? ParentCategoryId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive,
    IReadOnlyList<GuidedCategoryTreeResult> SubCategories,
    IReadOnlyList<GuidedProgramTreeResult> Programs);

/// <summary>
/// Represents a guided program with pricing.
/// </summary>
public sealed record GuidedProgramTreeResult(
    Guid ProgramId,
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<GuidedDurationTreeResult> Durations);

/// <summary>
/// Represents a guided duration-based price.
/// </summary>
public sealed record GuidedDurationTreeResult(
    Guid PricingId,
    Guid ProgramId,
    short DurationWeeks,
    decimal OriginalPrice,
    decimal? DiscountPercentage,
    decimal FinalPrice,
    string CurrencyCode,
    bool IsActive);
