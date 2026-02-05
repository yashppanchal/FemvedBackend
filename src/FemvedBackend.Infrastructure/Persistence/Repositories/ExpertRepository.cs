using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.UseCases.Experts;
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
            .FirstOrDefaultAsync(expert => expert.UserId == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Expert>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Experts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExpertProgramsResult>> ListActiveWithProgramsAsync(CancellationToken cancellationToken = default)
    {
        var experts = await _dbContext.Experts
            .AsNoTracking()
            .Where(expert => expert.IsVerified)
            .AsSplitQuery()
            .Select(expert => new
            {
                expert.UserId,
                expert.Bio,
                FirstName = expert.User != null ? expert.User.FirstName : string.Empty,
                LastName = expert.User != null ? expert.User.LastName : string.Empty,
                Programs = expert.ExpertProducts
                    .Where(expertProduct => expertProduct.IsActive
                        && expertProduct.Product != null
                        && expertProduct.Product.IsActive)
                    .GroupBy(expertProduct => new
                    {
                        expertProduct.ProductId,
                        expertProduct.Product!.Title,
                        expertProduct.Product.Description,
                        expertProduct.Product.ImageUrl
                    })
                    .Select(group => new
                    {
                        group.Key.ProductId,
                        group.Key.Title,
                        group.Key.Description,
                        group.Key.ImageUrl,
                        Durations = group.Select(duration => new
                            {
                                duration.DurationWeeks,
                                duration.OriginalPrice,
                                duration.DiscountPercentage,
                                duration.FinalPrice
                            })
                            .OrderBy(duration => duration.DurationWeeks)
                            .ToList()
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return experts
            .Select(expert => new ExpertProgramsResult(
                expert.UserId,
                BuildExpertName(expert.FirstName, expert.LastName),
                expert.Bio,
                expert.Programs
                    .Select(program => new ExpertProgramResult(
                        program.ProductId,
                        program.Title,
                        program.Description,
                        string.IsNullOrWhiteSpace(program.ImageUrl) ? null : program.ImageUrl,
                        program.Durations
                            .Select(duration => new ExpertProgramDurationResult(
                                duration.DurationWeeks,
                                duration.OriginalPrice,
                                duration.DiscountPercentage > 0 ? duration.DiscountPercentage : null,
                                duration.FinalPrice > 0 ? duration.FinalPrice : null))
                            .ToList()))
                    .ToList()))
            .ToList();
    }

    public async Task<ExpertProgramsResult?> GetActiveWithProgramsAsync(Guid expertId, CancellationToken cancellationToken = default)
    {
        var expert = await _dbContext.Experts
            .AsNoTracking()
            .Where(candidate => candidate.IsVerified && candidate.UserId == expertId)
            .AsSplitQuery()
            .Select(candidate => new
            {
                candidate.UserId,
                candidate.Bio,
                FirstName = candidate.User != null ? candidate.User.FirstName : string.Empty,
                LastName = candidate.User != null ? candidate.User.LastName : string.Empty,
                Programs = candidate.ExpertProducts
                    .Where(expertProduct => expertProduct.IsActive
                        && expertProduct.Product != null
                        && expertProduct.Product.IsActive)
                    .GroupBy(expertProduct => new
                    {
                        expertProduct.ProductId,
                        expertProduct.Product!.Title,
                        expertProduct.Product.Description,
                        expertProduct.Product.ImageUrl
                    })
                    .Select(group => new
                    {
                        group.Key.ProductId,
                        group.Key.Title,
                        group.Key.Description,
                        group.Key.ImageUrl,
                        Durations = group.Select(duration => new
                            {
                                duration.DurationWeeks,
                                duration.OriginalPrice,
                                duration.DiscountPercentage,
                                duration.FinalPrice
                            })
                            .OrderBy(duration => duration.DurationWeeks)
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (expert is null)
        {
            return null;
        }

        return new ExpertProgramsResult(
            expert.UserId,
            BuildExpertName(expert.FirstName, expert.LastName),
            expert.Bio,
            expert.Programs
                .Select(program => new ExpertProgramResult(
                    program.ProductId,
                    program.Title,
                    program.Description,
                    string.IsNullOrWhiteSpace(program.ImageUrl) ? null : program.ImageUrl,
                    program.Durations
                        .Select(duration => new ExpertProgramDurationResult(
                            duration.DurationWeeks,
                            duration.OriginalPrice,
                            duration.DiscountPercentage > 0 ? duration.DiscountPercentage : null,
                            duration.FinalPrice > 0 ? duration.FinalPrice : null))
                        .ToList()))
                .ToList());
    }

    public Task<bool> IsVerifiedAsync(Guid expertId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Experts
            .AsNoTracking()
            .AnyAsync(expert => expert.UserId == expertId && expert.IsVerified, cancellationToken);
    }

    private static string BuildExpertName(string firstName, string lastName)
    {
        return string.Join(" ", new[] { firstName, lastName }.Where(value => !string.IsNullOrWhiteSpace(value)));
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
