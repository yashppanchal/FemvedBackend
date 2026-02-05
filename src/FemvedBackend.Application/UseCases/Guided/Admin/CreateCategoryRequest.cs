namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to create a category.
/// </summary>
public sealed record CreateCategoryRequest(
    short DomainId,
    Guid? ParentCategoryId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive);
