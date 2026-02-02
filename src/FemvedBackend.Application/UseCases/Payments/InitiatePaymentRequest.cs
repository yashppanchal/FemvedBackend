namespace FemvedBackend.Application.UseCases.Payments;

/// <summary>
/// Represents a request to initiate a payment for an order.
/// </summary>
public sealed record InitiatePaymentRequest(
    Guid OrderId,
    short PaymentGatewayId,
    string PaymentGatewayName,
    string Currency,
    string? Reference);
