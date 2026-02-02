using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FemvedBackend.Infrastructure.Persistence.Repositories;

public sealed class UserProductAccessRepository : IUserProductAccessRepository
{
    private readonly AppDbContext _dbContext;

    public UserProductAccessRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UserProductAccess?> GetAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default)
    {
        return _dbContext.UserProductAccesses
            .FirstOrDefaultAsync(access => access.UserId == userId && access.ProductId == productId, cancellationToken);
    }

    public async Task AddAsync(UserProductAccess access, CancellationToken cancellationToken = default)
    {
        _dbContext.UserProductAccesses.Add(access);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserProductAccess access, CancellationToken cancellationToken = default)
    {
        _dbContext.UserProductAccesses.Update(access);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(UserProductAccess access, CancellationToken cancellationToken = default)
    {
        _dbContext.UserProductAccesses.Remove(access);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
