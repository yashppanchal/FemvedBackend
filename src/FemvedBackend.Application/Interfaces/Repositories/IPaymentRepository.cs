using FemvedBackend.Domain.Entities;

namespace FemvedBackend.Application.Interfaces.Repositories;

/// <summary>
/// Provides payment persistence operations.
/// </summary>
public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> ListByOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
}
