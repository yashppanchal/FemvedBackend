using FemvedBackend.Domain.Entities;

namespace FemvedBackend.Application.Interfaces.Repositories;

/// <summary>
/// Provides order persistence operations.
/// </summary>
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
}
