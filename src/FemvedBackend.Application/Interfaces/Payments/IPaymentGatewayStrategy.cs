using FemvedBackend.Application.Payments;

namespace FemvedBackend.Application.Interfaces.Payments;

/// <summary>
/// Defines a payment gateway strategy contract.
/// </summary>
public interface IPaymentGatewayStrategy
{
    string Name { get; }
    Task<PaymentResult> ChargeAsync(PaymentRequest request, CancellationToken cancellationToken = default);
    Task<PaymentResult> RefundAsync(RefundRequest request, CancellationToken cancellationToken = default);
}
