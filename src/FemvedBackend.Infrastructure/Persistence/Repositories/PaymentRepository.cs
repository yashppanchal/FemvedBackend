using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FemvedBackend.Infrastructure.Persistence.Repositories;

public sealed class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _dbContext;

    public PaymentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Payments
            .AsNoTracking()
            .FirstOrDefaultAsync(payment => payment.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> ListByOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments
            .AsNoTracking()
            .Where(payment => payment.OrderId == orderId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
