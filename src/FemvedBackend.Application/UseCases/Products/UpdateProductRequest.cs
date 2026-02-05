namespace FemvedBackend.Application.UseCases.Products;

/// <summary>
/// Represents a request to update a product.
/// </summary>
public sealed record UpdateProductRequest(
    Guid ProductId,
    short ProductTypeId,
    string Title,
    string? Description,
    string? ImageUrl);
