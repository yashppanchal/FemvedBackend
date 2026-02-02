using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a customer order.
/// </summary>
public class Order : Entity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
}
