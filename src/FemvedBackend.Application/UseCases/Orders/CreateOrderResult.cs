using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Application.UseCases.Orders;

/// <summary>
/// Represents the result of creating an order.
/// </summary>
public sealed record CreateOrderResult(
    Guid OrderId,
    OrderStatus Status,
    decimal TotalAmount);
