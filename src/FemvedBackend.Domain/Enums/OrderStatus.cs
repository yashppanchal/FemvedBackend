namespace FemvedBackend.Domain.Enums;

/// <summary>
/// Represents the lifecycle status of an order.
/// </summary>
public enum OrderStatus
{
    Draft = 0,
    PendingPayment = 1,
    Paid = 2,
    Cancelled = 3
}
