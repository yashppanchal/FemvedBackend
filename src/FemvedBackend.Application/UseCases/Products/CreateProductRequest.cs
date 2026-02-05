namespace FemvedBackend.Application.UseCases.Products;

/// <summary>
/// Represents a request to create a product.
/// </summary>
public sealed record CreateProductRequest(
    short ProductTypeId,
    string Title,
    string? Description,
    string? ImageUrl);
