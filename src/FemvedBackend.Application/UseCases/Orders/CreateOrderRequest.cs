namespace FemvedBackend.Application.UseCases.Orders;

/// <summary>
/// Represents a request to create a new order.
/// </summary>
public sealed record CreateOrderRequest(
    IReadOnlyList<CreateOrderItemRequest> Items,
    Guid? CouponId);
