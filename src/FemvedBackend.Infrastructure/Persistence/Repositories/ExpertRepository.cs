using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FemvedBackend.Infrastructure.Persistence.Repositories;

public sealed class ExpertRepository : IExpertRepository
{
    private readonly AppDbContext _dbContext;

    public ExpertRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Expert?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Experts
            .AsNoTracking()
            .FirstOrDefaultAsync(expert => expert.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Expert>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Experts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Expert expert, CancellationToken cancellationToken = default)
    {
        _dbContext.Experts.Add(expert);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Expert expert, CancellationToken cancellationToken = default)
    {
        _dbContext.Experts.Update(expert);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
