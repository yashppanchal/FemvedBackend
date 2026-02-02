using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a discount coupon.
/// </summary>
public class Coupon : Entity
{
    public string Code { get; set; } = string.Empty;
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset ValidTo { get; set; }
    public int? MaxUsages { get; set; }
    public ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
}
