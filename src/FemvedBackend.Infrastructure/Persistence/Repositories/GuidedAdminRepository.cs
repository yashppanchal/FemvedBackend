using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FemvedBackend.Infrastructure.Persistence.Repositories;

public sealed class GuidedAdminRepository : IGuidedAdminRepository
{
    private readonly AppDbContext _dbContext;

    public GuidedAdminRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> DomainNameExistsAsync(
        string name,
        short? excludeDomainId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Domains
            .AsNoTracking()
            .Where(domain => !excludeDomainId.HasValue || domain.Id != excludeDomainId.Value)
            .AnyAsync(domain => domain.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public Task<global::FemvedBackend.Domain.Entities.Domain?> GetDomainByIdAsync(
        short domainId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Domains
            .FirstOrDefaultAsync(domain => domain.Id == domainId, cancellationToken);
    }

    public Task<bool> DomainExistsAsync(short domainId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Domains
            .AsNoTracking()
            .AnyAsync(domain => domain.Id == domainId, cancellationToken);
    }

    public async Task AddDomainAsync(global::FemvedBackend.Domain.Entities.Domain domain, CancellationToken cancellationToken = default)
    {
        _dbContext.Domains.Add(domain);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateDomainAsync(global::FemvedBackend.Domain.Entities.Domain domain, CancellationToken cancellationToken = default)
    {
        _dbContext.Domains.Update(domain);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDomainAsync(short domainId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var categories = await _dbContext.Categories
                .Where(category => category.DomainId == domainId)
                .ToListAsync(cancellationToken);

            var categoryIds = categories.Select(category => category.Id).ToList();

            var programs = await _dbContext.Programs
                .Where(program => categoryIds.Contains(program.CategoryId))
                .ToListAsync(cancellationToken);

            var programIds = programs.Select(program => program.Id).ToList();

            var pricing = await _dbContext.ProgramPricing
                .Where(item => programIds.Contains(item.ProgramId))
                .ToListAsync(cancellationToken);

            if (pricing.Count > 0)
            {
                _dbContext.ProgramPricing.RemoveRange(pricing);
            }

            if (programs.Count > 0)
            {
                _dbContext.Programs.RemoveRange(programs);
            }

            if (categories.Count > 0)
            {
                _dbContext.Categories.RemoveRange(categories);
            }

            var domain = await _dbContext.Domains
                .FirstOrDefaultAsync(item => item.Id == domainId, cancellationToken);

            if (domain is not null)
            {
                _dbContext.Domains.Remove(domain);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Category?> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Categories
            .FirstOrDefaultAsync(category => category.Id == categoryId, cancellationToken);
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> CategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Categories
            .AsNoTracking()
            .AnyAsync(category => category.Id == categoryId, cancellationToken);
    }

    public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var categories = await _dbContext.Categories
                .AsNoTracking()
                .Select(category => new { category.Id, category.ParentCategoryId })
                .ToListAsync(cancellationToken);

            var categoryIds = new HashSet<Guid>();
            var queue = new Queue<Guid>();
            queue.Enqueue(categoryId);

            while (queue.Count > 0)
            {
                var currentId = queue.Dequeue();
                if (!categoryIds.Add(currentId))
                {
                    continue;
                }

                foreach (var child in categories.Where(category => category.ParentCategoryId == currentId))
                {
                    queue.Enqueue(child.Id);
                }
            }

            var programs = await _dbContext.Programs
                .Where(program => categoryIds.Contains(program.CategoryId))
                .ToListAsync(cancellationToken);

            var programIds = programs.Select(program => program.Id).ToList();

            var pricing = await _dbContext.ProgramPricing
                .Where(item => programIds.Contains(item.ProgramId))
                .ToListAsync(cancellationToken);

            if (pricing.Count > 0)
            {
                _dbContext.ProgramPricing.RemoveRange(pricing);
            }

            if (programs.Count > 0)
            {
                _dbContext.Programs.RemoveRange(programs);
            }

            var categoryEntities = await _dbContext.Categories
                .Where(category => categoryIds.Contains(category.Id))
                .ToListAsync(cancellationToken);

            if (categoryEntities.Count > 0)
            {
                _dbContext.Categories.RemoveRange(categoryEntities);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task AddProgramWithPricingAsync(
        global::FemvedBackend.Domain.Entities.Program program,
        IReadOnlyList<ProgramPricing> pricing,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _dbContext.Programs.Add(program);

            if (pricing.Count > 0)
            {
                _dbContext.ProgramPricing.AddRange(pricing);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public Task<global::FemvedBackend.Domain.Entities.Program?> GetProgramByIdAsync(
        Guid programId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Programs
            .FirstOrDefaultAsync(program => program.Id == programId, cancellationToken);
    }

    public async Task UpdateProgramAsync(
        global::FemvedBackend.Domain.Entities.Program program,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Programs.Update(program);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ProgramExistsAsync(Guid programId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Programs
            .AsNoTracking()
            .AnyAsync(program => program.Id == programId, cancellationToken);
    }

    public async Task DeleteProgramAsync(Guid programId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var pricing = await _dbContext.ProgramPricing
                .Where(item => item.ProgramId == programId)
                .ToListAsync(cancellationToken);

            if (pricing.Count > 0)
            {
                _dbContext.ProgramPricing.RemoveRange(pricing);
            }

            var program = await _dbContext.Programs
                .FirstOrDefaultAsync(item => item.Id == programId, cancellationToken);

            if (program is not null)
            {
                _dbContext.Programs.Remove(program);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task AddProgramPricingAsync(ProgramPricing pricing, CancellationToken cancellationToken = default)
    {
        _dbContext.ProgramPricing.Add(pricing);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProgramPricing>> GetProgramPricingByProgramIdAsync(
        Guid programId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProgramPricing
            .AsNoTracking()
            .Where(pricing => pricing.ProgramId == programId)
            .ToListAsync(cancellationToken);
    }

    public Task<ProgramPricing?> GetProgramPricingByIdAsync(Guid pricingId, CancellationToken cancellationToken = default)
    {
        return _dbContext.ProgramPricing
            .FirstOrDefaultAsync(pricing => pricing.Id == pricingId, cancellationToken);
    }

    public async Task UpdateProgramPricingAsync(ProgramPricing pricing, CancellationToken cancellationToken = default)
    {
        _dbContext.ProgramPricing.Update(pricing);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ProgramPricingDurationExistsAsync(
        Guid programId,
        short durationWeeks,
        Guid? excludePricingId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ProgramPricing
            .AsNoTracking()
            .Where(pricing => pricing.ProgramId == programId)
            .Where(pricing => !excludePricingId.HasValue || pricing.Id != excludePricingId.Value)
            .AnyAsync(pricing => pricing.DurationWeeks == durationWeeks, cancellationToken);
    }

    public async Task AddExpertAsync(Expert expert, CancellationToken cancellationToken = default)
    {
        _dbContext.Experts.Add(expert);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Expert?> GetExpertByIdAsync(Guid expertId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Experts
            .FirstOrDefaultAsync(expert => expert.UserId == expertId, cancellationToken);
    }

    public async Task UpdateExpertAsync(Expert expert, CancellationToken cancellationToken = default)
    {
        _dbContext.Experts.Update(expert);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExpertHasProgramsAsync(Guid expertId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Programs
            .AsNoTracking()
            .AnyAsync(program => program.ExpertId == expertId, cancellationToken);
    }
}
