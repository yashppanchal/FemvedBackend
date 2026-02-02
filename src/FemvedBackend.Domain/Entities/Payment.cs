using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a payment for an order.
/// </summary>
public class Payment : Entity
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public short PaymentGatewayId { get; set; }
    public PaymentGateway? PaymentGateway { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset? PaidAt { get; set; }
    public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}
