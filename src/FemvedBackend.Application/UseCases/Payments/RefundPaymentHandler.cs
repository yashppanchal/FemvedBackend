using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Payments;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.Payments;
using FemvedBackend.Domain.Entities;
using FemvedBackend.Domain.Enums;
using Microsoft.Extensions.Logging;
namespace FemvedBackend.Application.UseCases.Payments;

/// <summary>
/// Handles payment refunds via a gateway strategy.
/// </summary>
public sealed class RefundPaymentHandler
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IReadOnlyCollection<IPaymentGatewayStrategy> _strategies;
    private readonly ILogger<RefundPaymentHandler> _logger;

    public RefundPaymentHandler(
        IPaymentRepository paymentRepository,
        IEnumerable<IPaymentGatewayStrategy> strategies,
        ILogger<RefundPaymentHandler> logger)
    {
        _paymentRepository = paymentRepository;
        _strategies = strategies.ToArray();
        _logger = logger;
    }

    public async Task<RefundPaymentResult> HandleAsync(
        RefundPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Refund payment requested for {PaymentId} via {Gateway} for amount {Amount}", request.PaymentId, request.PaymentGatewayName, request.Amount);

        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken)
            ?? throw new ValidationException("paymentId", "Payment not found.");

        if (payment.Status != PaymentStatus.Captured)
        {
            _logger.LogWarning("Refund payment failed for {PaymentId}: status {Status}", payment.Id, payment.Status);
            throw new ValidationException("paymentId", "Only captured payments can be refunded.");
        }

        var strategy = ResolveStrategy(request.PaymentGatewayName);
        var refundRequest = new RefundRequest(payment.Id, request.Amount, request.Reason);
        var refundResult = await strategy.RefundAsync(refundRequest, cancellationToken);

        var refund = new Refund
        {
            Id = Guid.NewGuid(),
            PaymentId = payment.Id,
            Status = refundResult.IsSuccessful ? RefundStatus.Completed : RefundStatus.Declined,
            Amount = request.Amount,
            RequestedAt = DateTimeOffset.UtcNow,
            CompletedAt = refundResult.IsSuccessful ? DateTimeOffset.UtcNow : null
        };

        payment.Refunds.Add(refund);
        payment.Status = refundResult.IsSuccessful ? PaymentStatus.Refunded : payment.Status;

        await _paymentRepository.UpdateAsync(payment, cancellationToken);

        _logger.LogInformation("Refund payment completed for {PaymentId} with payment status {PaymentStatus} and refund status {RefundStatus}", payment.Id, payment.Status, refund.Status);

        return new RefundPaymentResult(payment.Id, payment.Status, refund.Status, refundResult.ProviderReference);
    }

    private IPaymentGatewayStrategy ResolveStrategy(string gatewayName)
    {
        var strategy = _strategies.FirstOrDefault(item =>
            string.Equals(item.Name, gatewayName, StringComparison.OrdinalIgnoreCase));

        return strategy ?? throw new ValidationException("paymentGateway", "Payment gateway strategy not available.");
    }
}
