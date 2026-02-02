namespace FemvedBackend.Api.Contracts.Payments;

/// <summary>
/// Represents an initiate payment response.
/// </summary>
public sealed record InitiatePaymentResponseDto(
    Guid PaymentId,
    string Status,
    string? ProviderReference);
