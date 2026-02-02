namespace FemvedBackend.Api.Contracts.Payments;

/// <summary>
/// Represents a request to refund a payment.
/// </summary>
public sealed record RefundPaymentRequestDto(
    Guid PaymentId,
    string PaymentGatewayName,
    decimal Amount,
    string? Reason);
