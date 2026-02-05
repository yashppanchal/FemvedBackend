namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to delete a category.
/// </summary>
public sealed record DeleteCategoryRequest(Guid CategoryId);
