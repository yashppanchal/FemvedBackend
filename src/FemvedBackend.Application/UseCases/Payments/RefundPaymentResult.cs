using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Application.UseCases.Payments;

/// <summary>
/// Represents the outcome of a payment refund.
/// </summary>
public sealed record RefundPaymentResult(
    Guid PaymentId,
    PaymentStatus Status,
    RefundStatus RefundStatus,
    string? ProviderReference);
