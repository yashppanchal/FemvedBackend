using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FemvedBackend.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public OrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Where(order => order.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
