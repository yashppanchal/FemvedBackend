namespace FemvedBackend.Application.UseCases.Payments;

/// <summary>
/// Represents a request to refund a payment.
/// </summary>
public sealed record RefundPaymentRequest(
    Guid PaymentId,
    string PaymentGatewayName,
    decimal Amount,
    string? Reason);
