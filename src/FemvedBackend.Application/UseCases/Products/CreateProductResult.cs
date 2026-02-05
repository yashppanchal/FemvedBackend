namespace FemvedBackend.Application.UseCases.Products;

/// <summary>
/// Represents the result of creating a product.
/// </summary>
public sealed record CreateProductResult(
    Guid ProductId,
    short ProductTypeId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive);
