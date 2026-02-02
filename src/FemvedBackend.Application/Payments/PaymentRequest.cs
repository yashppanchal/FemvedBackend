namespace FemvedBackend.Application.Payments;

/// <summary>
/// Represents a payment charge request.
/// </summary>
public sealed record PaymentRequest(
    Guid OrderId,
    Guid UserId,
    decimal Amount,
    string Currency,
    string? Reference);
