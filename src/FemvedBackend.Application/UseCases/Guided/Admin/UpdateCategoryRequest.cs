namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to update a category.
/// </summary>
public sealed record UpdateCategoryRequest(
    Guid CategoryId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive);
