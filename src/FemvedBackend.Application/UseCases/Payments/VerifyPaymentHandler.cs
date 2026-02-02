using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Enums;
using Microsoft.Extensions.Logging;
namespace FemvedBackend.Application.UseCases.Payments;

/// <summary>
/// Handles verification of a payment status.
/// </summary>
public sealed class VerifyPaymentHandler
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<VerifyPaymentHandler> _logger;

    public VerifyPaymentHandler(IPaymentRepository paymentRepository, ILogger<VerifyPaymentHandler> logger)
    {
        _paymentRepository = paymentRepository;
        _logger = logger;
    }

    public async Task<VerifyPaymentResult> HandleAsync(
        VerifyPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Verify payment requested for {PaymentId}", request.PaymentId);

        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken)
            ?? throw new ValidationException("paymentId", "Payment not found.");

        payment.Status = request.IsSuccessful ? PaymentStatus.Captured : PaymentStatus.Failed;
        payment.PaidAt ??= request.IsSuccessful ? DateTimeOffset.UtcNow : null;

        await _paymentRepository.UpdateAsync(payment, cancellationToken);

        _logger.LogInformation("Verify payment completed for {PaymentId} with status {Status}", payment.Id, payment.Status);

        return new VerifyPaymentResult(payment.Id, payment.Status, request.ProviderReference);
    }
}
