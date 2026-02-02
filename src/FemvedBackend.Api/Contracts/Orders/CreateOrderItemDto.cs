namespace FemvedBackend.Api.Contracts.Orders;

/// <summary>
/// Represents an order item payload for API requests.
/// </summary>
public sealed record CreateOrderItemDto(
    Guid ProductVariantId,
    int Quantity,
    decimal UnitPrice);
