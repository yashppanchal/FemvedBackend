using FemvedBackend.Domain.Entities;

namespace FemvedBackend.Application.Interfaces.Repositories;

/// <summary>
/// Provides persistence operations for user product access.
/// </summary>
public interface IUserProductAccessRepository
{
    Task<UserProductAccess?> GetAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default);
    Task AddAsync(UserProductAccess access, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserProductAccess access, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserProductAccess access, CancellationToken cancellationToken = default);
}
