namespace FemvedBackend.Application.UseCases.Products;

/// <summary>
/// Represents the result of updating a product.
/// </summary>
public sealed record UpdateProductResult(
    Guid ProductId,
    short ProductTypeId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive);
