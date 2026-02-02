namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents usage of a coupon by a user.
/// </summary>
public class CouponUsage : Entity
{
    public Guid CouponId { get; set; }
    public Coupon? Coupon { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public DateTimeOffset UsedAt { get; set; }
}
