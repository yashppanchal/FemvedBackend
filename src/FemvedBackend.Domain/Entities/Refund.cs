using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a refund against a payment.
/// </summary>
public class Refund : Entity
{
    public Guid PaymentId { get; set; }
    public Payment? Payment { get; set; }
    public RefundStatus Status { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
}
