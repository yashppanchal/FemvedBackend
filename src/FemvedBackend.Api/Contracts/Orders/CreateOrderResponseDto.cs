namespace FemvedBackend.Api.Contracts.Orders;

/// <summary>
/// Represents an order response payload.
/// </summary>
public sealed record CreateOrderResponseDto(
    Guid OrderId,
    string Status,
    decimal TotalAmount);
