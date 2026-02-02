using FemvedBackend.Api.Contracts.Orders;
using FemvedBackend.Application.UseCases.Orders;
using Microsoft.AspNetCore.Mvc;

namespace FemvedBackend.Api.Controllers;

/// <summary>
/// Exposes order-related endpoints.
/// </summary>
[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _createOrderHandler;

    public OrdersController(CreateOrderHandler createOrderHandler)
    {
        _createOrderHandler = createOrderHandler;
    }

    /// <summary>
    /// Creates a new order for the current user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateOrderResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateOrderResponseDto>> CreateOrder(
        [FromBody] CreateOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new CreateOrderRequest(
            request.Items.Select(item =>
                new CreateOrderItemRequest(item.ProductVariantId, item.Quantity, item.UnitPrice)).ToList(),
            request.CouponId);

        var result = await _createOrderHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new CreateOrderResponseDto(
            result.OrderId,
            result.Status.ToString(),
            result.TotalAmount);

        return Ok(response);
    }
}
