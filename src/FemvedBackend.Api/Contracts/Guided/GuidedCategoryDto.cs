namespace FemvedBackend.Api.Contracts.Guided;

/// <summary>
/// Represents a guided category with subcategories and programs.
/// </summary>
public sealed record GuidedCategoryDto(
    Guid CategoryId,
    short DomainId,
    Guid? ParentCategoryId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive,
    IReadOnlyList<GuidedCategoryDto> SubCategories,
    IReadOnlyList<GuidedProgramDto> Programs);
