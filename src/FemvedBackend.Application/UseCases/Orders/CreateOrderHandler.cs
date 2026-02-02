using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.Exceptions;
using FemvedBackend.Domain.Entities;
using FemvedBackend.Domain.Enums;
namespace FemvedBackend.Application.UseCases.Orders;

/// <summary>
/// Handles creation of new orders for the current user.
/// </summary>
public sealed class CreateOrderHandler
{
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IOrderRepository _orderRepository;

    public CreateOrderHandler(
        ICurrentUserContext currentUserContext,
        IOrderRepository orderRepository)
    {
        _currentUserContext = currentUserContext;
        _orderRepository = orderRepository;
    }

    public async Task<CreateOrderResult> HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        if (!_currentUserContext.IsAuthenticated || _currentUserContext.UserId is null)
        {
            throw new ValidationException("user", "User must be authenticated to create an order.");
        }

        var items = request.Items.Select(item => new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductVariantId = item.ProductVariantId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        }).ToList();

        var totalAmount = items.Sum(item => item.UnitPrice * item.Quantity);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserContext.UserId.Value,
            Status = OrderStatus.PendingPayment,
            TotalAmount = totalAmount,
            Items = items
        };

        await _orderRepository.AddAsync(order, cancellationToken);

        return new CreateOrderResult(order.Id, order.Status, order.TotalAmount);
    }
}
