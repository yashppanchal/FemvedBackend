namespace FemvedBackend.Api.Contracts.Payments;

/// <summary>
/// Represents a refund payment response.
/// </summary>
public sealed record RefundPaymentResponseDto(
    Guid PaymentId,
    string Status,
    string RefundStatus,
    string? ProviderReference);
