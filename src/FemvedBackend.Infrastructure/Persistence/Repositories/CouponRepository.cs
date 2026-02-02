using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FemvedBackend.Infrastructure.Persistence.Repositories;

public sealed class CouponRepository : ICouponRepository
{
    private readonly AppDbContext _dbContext;

    public CouponRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Coupon?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(coupon => coupon.Id == id, cancellationToken);
    }

    public Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return _dbContext.Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(coupon => coupon.Code == code, cancellationToken);
    }

    public async Task<IReadOnlyList<Coupon>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Coupons
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        _dbContext.Coupons.Add(coupon);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        _dbContext.Coupons.Update(coupon);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
