namespace FemvedBackend.Application.UseCases.Payments;

/// <summary>
/// Represents a request to verify a payment.
/// </summary>
public sealed record VerifyPaymentRequest(
    Guid PaymentId,
    bool IsSuccessful,
    string? ProviderReference);
