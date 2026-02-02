namespace FemvedBackend.Application.Payments;

/// <summary>
/// Represents a refund request.
/// </summary>
public sealed record RefundRequest(
    Guid PaymentId,
    decimal Amount,
    string? Reason);
