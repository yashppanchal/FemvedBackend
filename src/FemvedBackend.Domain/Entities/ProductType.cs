namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a product classification.
/// </summary>
public class ProductType
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
