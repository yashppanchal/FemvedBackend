using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Payments;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.Payments;
using FemvedBackend.Domain.Entities;
using FemvedBackend.Domain.Enums;
using Microsoft.Extensions.Logging;
namespace FemvedBackend.Application.UseCases.Payments;

/// <summary>
/// Handles payment initiation for orders.
/// </summary>
public sealed class InitiatePaymentHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IReadOnlyCollection<IPaymentGatewayStrategy> _strategies;
    private readonly ILogger<InitiatePaymentHandler> _logger;

    public InitiatePaymentHandler(
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository,
        IEnumerable<IPaymentGatewayStrategy> strategies,
        ILogger<InitiatePaymentHandler> logger)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _strategies = strategies.ToArray();
        _logger = logger;
    }

    public async Task<InitiatePaymentResult> HandleAsync(
        InitiatePaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Initiate payment requested for order {OrderId} via {Gateway}", request.OrderId, request.PaymentGatewayName);

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new ValidationException("orderId", "Order not found.");

        if (order.Status != OrderStatus.PendingPayment)
        {
            _logger.LogWarning("Initiate payment failed for order {OrderId}: invalid status {Status}", order.Id, order.Status);
            throw new ValidationException("orderId", "Order is not eligible for payment.");
        }

        var strategy = ResolveStrategy(request.PaymentGatewayName);

        var gatewayRequest = new PaymentRequest(
            order.Id,
            order.UserId,
            order.TotalAmount,
            request.Currency,
            request.Reference);

        var gatewayResult = await strategy.ChargeAsync(gatewayRequest, cancellationToken);

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            PaymentGatewayId = request.PaymentGatewayId,
            Status = gatewayResult.IsSuccessful ? PaymentStatus.Captured : PaymentStatus.Failed,
            Amount = order.TotalAmount,
            PaidAt = gatewayResult.IsSuccessful ? DateTimeOffset.UtcNow : null
        };

        await _paymentRepository.AddAsync(payment, cancellationToken);

        _logger.LogInformation("Initiate payment completed for order {OrderId} with status {Status}", order.Id, payment.Status);

        return new InitiatePaymentResult(payment.Id, payment.Status, gatewayResult.ProviderReference);
    }

    private IPaymentGatewayStrategy ResolveStrategy(string gatewayName)
    {
        var strategy = _strategies.FirstOrDefault(item =>
            string.Equals(item.Name, gatewayName, StringComparison.OrdinalIgnoreCase));

        return strategy ?? throw new ValidationException("paymentGateway", "Payment gateway strategy not available.");
    }
}
