namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of updating a category.
/// </summary>
public sealed record UpdateCategoryResult(
    Guid Id,
    short DomainId,
    Guid? ParentCategoryId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive);
