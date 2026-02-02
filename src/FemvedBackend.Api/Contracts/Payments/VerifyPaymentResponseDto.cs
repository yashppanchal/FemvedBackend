namespace FemvedBackend.Api.Contracts.Payments;

/// <summary>
/// Represents a verify payment response.
/// </summary>
public sealed record VerifyPaymentResponseDto(
    Guid PaymentId,
    string Status,
    string? ProviderReference);
