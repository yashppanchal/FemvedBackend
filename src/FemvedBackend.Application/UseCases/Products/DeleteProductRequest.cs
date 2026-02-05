namespace FemvedBackend.Application.UseCases.Products;

/// <summary>
/// Represents a request to delete a product.
/// </summary>
public sealed record DeleteProductRequest(Guid ProductId);
