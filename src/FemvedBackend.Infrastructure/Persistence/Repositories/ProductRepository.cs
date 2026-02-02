using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FemvedBackend.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
    {
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
