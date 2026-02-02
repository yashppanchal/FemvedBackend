using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Application.UseCases.Payments;

/// <summary>
/// Represents the outcome of verifying a payment.
/// </summary>
public sealed record VerifyPaymentResult(
    Guid PaymentId,
    PaymentStatus Status,
    string? ProviderReference);
