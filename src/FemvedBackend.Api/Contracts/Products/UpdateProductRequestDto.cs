namespace FemvedBackend.Api.Contracts.Products;

/// <summary>
/// Represents a request to update a product.
/// </summary>
public sealed record UpdateProductRequestDto(
    short ProductTypeId,
    string Title,
    string? Description,
    string? ImageUrl);
