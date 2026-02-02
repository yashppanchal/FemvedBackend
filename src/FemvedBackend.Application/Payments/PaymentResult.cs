namespace FemvedBackend.Application.Payments;

/// <summary>
/// Represents the outcome of a payment gateway operation.
/// </summary>
public sealed record PaymentResult(
    bool IsSuccessful,
    string? ProviderReference,
    string? FailureReason);
