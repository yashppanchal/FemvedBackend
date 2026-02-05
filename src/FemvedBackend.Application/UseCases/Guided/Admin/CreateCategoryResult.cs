namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of creating a category.
/// </summary>
public sealed record CreateCategoryResult(
    Guid Id,
    short DomainId,
    Guid? ParentCategoryId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive);
