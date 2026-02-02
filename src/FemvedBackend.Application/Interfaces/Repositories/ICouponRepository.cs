using FemvedBackend.Domain.Entities;

namespace FemvedBackend.Application.Interfaces.Repositories;

/// <summary>
/// Provides coupon persistence operations.
/// </summary>
public interface ICouponRepository
{
    Task<Coupon?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Coupon>> ListAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Coupon coupon, CancellationToken cancellationToken = default);
    Task UpdateAsync(Coupon coupon, CancellationToken cancellationToken = default);
}
