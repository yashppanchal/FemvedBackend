using FemvedBackend.Domain.Entities;
using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Application.Interfaces.Repositories;

/// <summary>
/// Provides role persistence operations.
/// </summary>
public interface IRoleRepository
{
    Task<Role?> GetByTypeAsync(RoleType roleType, CancellationToken cancellationToken = default);
}
