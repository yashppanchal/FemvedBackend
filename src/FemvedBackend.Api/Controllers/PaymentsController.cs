using FemvedBackend.Api.Contracts.Payments;
using FemvedBackend.Application.UseCases.Payments;
using Microsoft.AspNetCore.Mvc;

namespace FemvedBackend.Api.Controllers;

/// <summary>
/// Exposes payment-related endpoints.
/// </summary>
[ApiController]
[Route("api/payments")]
public sealed class PaymentsController : ControllerBase
{
    private readonly InitiatePaymentHandler _initiatePaymentHandler;
    private readonly VerifyPaymentHandler _verifyPaymentHandler;
    private readonly RefundPaymentHandler _refundPaymentHandler;

    public PaymentsController(
        InitiatePaymentHandler initiatePaymentHandler,
        VerifyPaymentHandler verifyPaymentHandler,
        RefundPaymentHandler refundPaymentHandler)
    {
        _initiatePaymentHandler = initiatePaymentHandler;
        _verifyPaymentHandler = verifyPaymentHandler;
        _refundPaymentHandler = refundPaymentHandler;
    }

    /// <summary>
    /// Initiates a payment for an order.
    /// </summary>
    [HttpPost("initiate")]
    [ProducesResponseType(typeof(InitiatePaymentResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InitiatePaymentResponseDto>> InitiatePayment(
        [FromBody] InitiatePaymentRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new InitiatePaymentRequest(
            request.OrderId,
            request.PaymentGatewayId,
            request.PaymentGatewayName,
            request.Currency,
            request.Reference);

        var result = await _initiatePaymentHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new InitiatePaymentResponseDto(
            result.PaymentId,
            result.Status.ToString(),
            result.ProviderReference);

        return Ok(response);
    }

    /// <summary>
    /// Verifies a payment outcome.
    /// </summary>
    [HttpPost("verify")]
    [ProducesResponseType(typeof(VerifyPaymentResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VerifyPaymentResponseDto>> VerifyPayment(
        [FromBody] VerifyPaymentRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new VerifyPaymentRequest(
            request.PaymentId,
            request.IsSuccessful,
            request.ProviderReference);

        var result = await _verifyPaymentHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new VerifyPaymentResponseDto(
            result.PaymentId,
            result.Status.ToString(),
            result.ProviderReference);

        return Ok(response);
    }

    /// <summary>
    /// Refunds a payment.
    /// </summary>
    [HttpPost("refund")]
    [ProducesResponseType(typeof(RefundPaymentResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RefundPaymentResponseDto>> RefundPayment(
        [FromBody] RefundPaymentRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new RefundPaymentRequest(
            request.PaymentId,
            request.PaymentGatewayName,
            request.Amount,
            request.Reason);

        var result = await _refundPaymentHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new RefundPaymentResponseDto(
            result.PaymentId,
            result.Status.ToString(),
            result.RefundStatus.ToString(),
            result.ProviderReference);

        return Ok(response);
    }
}
