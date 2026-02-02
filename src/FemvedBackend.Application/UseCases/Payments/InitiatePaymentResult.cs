using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Application.UseCases.Payments;

/// <summary>
/// Represents the outcome of a payment initiation.
/// </summary>
public sealed record InitiatePaymentResult(
    Guid PaymentId,
    PaymentStatus Status,
    string? ProviderReference);
