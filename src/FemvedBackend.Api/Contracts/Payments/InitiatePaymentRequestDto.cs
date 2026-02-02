namespace FemvedBackend.Api.Contracts.Payments;

/// <summary>
/// Represents a request to initiate a payment.
/// </summary>
public sealed record InitiatePaymentRequestDto(
    Guid OrderId,
    short PaymentGatewayId,
    string PaymentGatewayName,
    string Currency,
    string? Reference);
