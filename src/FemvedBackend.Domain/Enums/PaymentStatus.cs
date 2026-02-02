namespace FemvedBackend.Domain.Enums;

/// <summary>
/// Represents the status of a payment.
/// </summary>
public enum PaymentStatus
{
    Pending = 0,
    Authorized = 1,
    Captured = 2,
    Failed = 3,
    Refunded = 4
}
