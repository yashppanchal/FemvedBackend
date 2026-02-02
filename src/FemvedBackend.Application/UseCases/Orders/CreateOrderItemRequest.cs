namespace FemvedBackend.Application.UseCases.Orders;

/// <summary>
/// Represents an item to be included in an order.
/// </summary>
public sealed record CreateOrderItemRequest(
    Guid ProductVariantId,
    int Quantity,
    decimal UnitPrice);
