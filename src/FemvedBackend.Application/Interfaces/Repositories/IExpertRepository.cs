using FemvedBackend.Domain.Entities;

namespace FemvedBackend.Application.Interfaces.Repositories;

/// <summary>
/// Provides expert persistence operations.
/// </summary>
public interface IExpertRepository
{
    Task<Expert?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Expert>> ListAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Expert expert, CancellationToken cancellationToken = default);
    Task UpdateAsync(Expert expert, CancellationToken cancellationToken = default);
}
