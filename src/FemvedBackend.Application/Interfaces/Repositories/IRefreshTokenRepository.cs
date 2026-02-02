using FemvedBackend.Domain.Entities;

namespace FemvedBackend.Application.Interfaces.Repositories;

/// <summary>
/// Provides refresh token persistence operations.
/// </summary>
public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}
