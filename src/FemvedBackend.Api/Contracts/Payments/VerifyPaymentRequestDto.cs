namespace FemvedBackend.Api.Contracts.Payments;

/// <summary>
/// Represents a request to verify a payment.
/// </summary>
public sealed record VerifyPaymentRequestDto(
    Guid PaymentId,
    bool IsSuccessful,
    string? ProviderReference);
