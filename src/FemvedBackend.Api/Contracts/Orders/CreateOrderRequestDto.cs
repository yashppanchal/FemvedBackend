namespace FemvedBackend.Api.Contracts.Orders;

/// <summary>
/// Represents a request to create a new order.
/// </summary>
public sealed record CreateOrderRequestDto(
    IReadOnlyList<CreateOrderItemDto> Items,
    Guid? CouponId);
