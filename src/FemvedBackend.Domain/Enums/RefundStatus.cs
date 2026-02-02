namespace FemvedBackend.Domain.Enums;

/// <summary>
/// Represents the status of a refund.
/// </summary>
public enum RefundStatus
{
    Requested = 0,
    Approved = 1,
    Declined = 2,
    Completed = 3
}
