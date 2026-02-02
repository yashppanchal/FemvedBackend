namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a purchasable variant of a product.
/// </summary>
public class ProductVariant : Entity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
