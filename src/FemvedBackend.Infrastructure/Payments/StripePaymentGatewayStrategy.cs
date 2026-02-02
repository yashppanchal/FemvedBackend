using FemvedBackend.Application.Interfaces.Payments;
using FemvedBackend.Application.Payments;

namespace FemvedBackend.Infrastructure.Payments;

public sealed class StripePaymentGatewayStrategy : IPaymentGatewayStrategy
{
    public string Name => "Stripe";

    public Task<PaymentResult> ChargeAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var result = new PaymentResult(true, Guid.NewGuid().ToString("N"), null);
        return Task.FromResult(result);
    }

    public Task<PaymentResult> RefundAsync(RefundRequest request, CancellationToken cancellationToken = default)
    {
        var result = new PaymentResult(true, Guid.NewGuid().ToString("N"), null);
        return Task.FromResult(result);
    }
}
