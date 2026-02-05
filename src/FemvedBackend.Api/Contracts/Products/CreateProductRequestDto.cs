namespace FemvedBackend.Api.Contracts.Products;

/// <summary>
/// Represents a request to create a product.
/// </summary>
public sealed record CreateProductRequestDto(
    short ProductTypeId,
    string Title,
    string? Description,
    string? ImageUrl);
