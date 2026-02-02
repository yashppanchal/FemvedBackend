namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a line item within an order.
/// </summary>
public class OrderItem : Entity
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid ProductVariantId { get; set; }
    public ProductVariant? ProductVariant { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
