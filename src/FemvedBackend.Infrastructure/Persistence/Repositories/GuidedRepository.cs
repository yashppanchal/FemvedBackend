using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.UseCases.Guided;
using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FemvedBackend.Infrastructure.Persistence.Repositories;

public sealed class GuidedRepository : IGuidedRepository
{
    private readonly AppDbContext _dbContext;

    public GuidedRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<GuidedDomainTreeResult>> GetDomainTreeAsync(CancellationToken cancellationToken = default)
    {
        return await BuildDomainTreeAsync(null, cancellationToken);
    }

    public async Task<IReadOnlyList<GuidedDomainTreeResult>> GetDomainTreeByCategoryAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        return await BuildDomainTreeAsync(categoryId, cancellationToken);
    }

    public async Task<IReadOnlyList<GuidedDomainTreeResult>> GetDomainTreeBySubCategoryAsync(
        Guid subCategoryId,
        CancellationToken cancellationToken = default)
    {
        return await BuildDomainTreeAsync(subCategoryId, cancellationToken);
    }

    public async Task<IReadOnlyList<GuidedProgramFlatResult>> GetAllProgramsFlatAsync(
        CancellationToken cancellationToken = default)
    {
        var programs = await _dbContext.Programs
            .AsNoTracking()
            .Select(program => new
            {
                program.Id,
                program.CategoryId,
                program.ExpertId,
                program.Title,
                program.Description,
                program.ImageUrl,
                program.IsActive,
                program.CreatedAt,
                ExpertBio = program.Expert != null ? program.Expert.Bio : null,
                ExpertSpecialization = program.Expert != null ? program.Expert.Specialization : null,
                ExpertRating = program.Expert != null ? program.Expert.Rating : 0m,
                ExpertIsVerified = program.Expert != null && program.Expert.IsVerified,
                Durations = program.Pricing
                    .Select(duration => new
                    {
                        duration.Id,
                        duration.ProgramId,
                        duration.DurationWeeks,
                        duration.OriginalPrice,
                        duration.DiscountPercentage,
                        duration.FinalPrice,
                        duration.CurrencyCode,
                        duration.IsActive
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return programs
            .Select(program => new GuidedProgramFlatResult(
                program.Id,
                program.CategoryId,
                program.ExpertId,
                program.ExpertBio,
                program.ExpertSpecialization,
                program.ExpertRating,
                program.ExpertIsVerified,
                program.Title,
                program.Description,
                string.IsNullOrWhiteSpace(program.ImageUrl) ? null : program.ImageUrl,
                program.IsActive,
                program.CreatedAt,
                program.Durations
                    .Select(duration => new GuidedDurationFlatResult(
                        duration.Id,
                        duration.ProgramId,
                        duration.DurationWeeks,
                        duration.OriginalPrice,
                        duration.DiscountPercentage,
                        duration.FinalPrice,
                        duration.CurrencyCode,
                        duration.IsActive))
                    .ToList()))
            .ToList();
    }

    public async Task<GuidedProgramDetailsResult?> GetProgramDetailsAsync(
        Guid programId,
        CancellationToken cancellationToken = default)
    {
        var program = await _dbContext.Programs
            .AsNoTracking()
            .Where(candidate => candidate.Id == programId)
            .Select(candidate => new
            {
                candidate.Id,
                candidate.CategoryId,
                candidate.ExpertId,
                candidate.Title,
                candidate.Description,
                candidate.ImageUrl,
                candidate.IsActive,
                candidate.CreatedAt,
                Category = candidate.Category == null
                    ? null
                    : new
                    {
                        candidate.Category.Id,
                        candidate.Category.DomainId,
                        candidate.Category.ParentCategoryId,
                        candidate.Category.Name,
                        candidate.Category.Description,
                        candidate.Category.DisplayOrder,
                        candidate.Category.IsActive
                    },
                Expert = candidate.Expert == null
                    ? null
                    : new
                    {
                        candidate.Expert.UserId,
                        candidate.Expert.Bio,
                        candidate.Expert.Specialization,
                        candidate.Expert.Rating,
                        candidate.Expert.IsVerified
                    },
                Durations = candidate.Pricing
                    .Select(duration => new
                    {
                        duration.Id,
                        duration.ProgramId,
                        duration.DurationWeeks,
                        duration.OriginalPrice,
                        duration.DiscountPercentage,
                        duration.FinalPrice,
                        duration.CurrencyCode,
                        duration.IsActive
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (program is null || program.Category is null || program.Expert is null)
        {
            return null;
        }

        return new GuidedProgramDetailsResult(
            program.Id,
            program.CategoryId,
            new GuidedCategoryDetailsResult(
                program.Category.Id,
                program.Category.DomainId,
                program.Category.ParentCategoryId,
                program.Category.Name,
                program.Category.Description,
                program.Category.DisplayOrder,
                program.Category.IsActive),
            program.ExpertId,
            new GuidedExpertDetailsResult(
                program.Expert.UserId,
                program.Expert.Bio,
                program.Expert.Specialization,
                program.Expert.Rating,
                program.Expert.IsVerified),
            program.Title,
            program.Description,
            string.IsNullOrWhiteSpace(program.ImageUrl) ? null : program.ImageUrl,
            program.IsActive,
            program.CreatedAt,
            program.Durations
                .Select(duration => new GuidedDurationDetailsResult(
                    duration.Id,
                    duration.ProgramId,
                    duration.DurationWeeks,
                    duration.OriginalPrice,
                    duration.DiscountPercentage,
                    duration.FinalPrice,
                    duration.CurrencyCode,
                    duration.IsActive))
                .ToList());
    }

    public async Task<GuidedExpertProgramsResult?> GetExpertWithProgramsAsync(
        Guid expertId,
        CancellationToken cancellationToken = default)
    {
        var expert = await _dbContext.Experts
            .AsNoTracking()
            .Where(candidate => candidate.UserId == expertId)
            .Select(candidate => new
            {
                candidate.UserId,
                candidate.Bio,
                candidate.Specialization,
                candidate.Rating,
                candidate.IsVerified
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (expert is null)
        {
            return null;
        }

        var programs = await _dbContext.Programs
            .AsNoTracking()
            .Where(program => program.ExpertId == expert.UserId)
            .Select(program => new
            {
                program.Id,
                program.CategoryId,
                program.ExpertId,
                program.Title,
                program.Description,
                program.ImageUrl,
                program.IsActive,
                program.CreatedAt,
                Durations = program.Pricing
                    .Select(duration => new
                    {
                        duration.Id,
                        duration.ProgramId,
                        duration.DurationWeeks,
                        duration.OriginalPrice,
                        duration.DiscountPercentage,
                        duration.FinalPrice,
                        duration.CurrencyCode,
                        duration.IsActive
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return new GuidedExpertProgramsResult(
            expert.UserId,
            expert.Bio,
            expert.Specialization,
            expert.Rating,
            expert.IsVerified,
            programs
                .Select(program => new GuidedExpertProgramResult(
                    program.Id,
                    program.CategoryId,
                    program.ExpertId,
                    program.Title,
                    program.Description,
                    string.IsNullOrWhiteSpace(program.ImageUrl) ? null : program.ImageUrl,
                    program.IsActive,
                    program.CreatedAt,
                    program.Durations
                        .Select(duration => new GuidedExpertProgramDurationResult(
                            duration.Id,
                            duration.ProgramId,
                            duration.DurationWeeks,
                            duration.OriginalPrice,
                            duration.DiscountPercentage,
                            duration.FinalPrice,
                            duration.CurrencyCode,
                            duration.IsActive))
                        .ToList()))
                .ToList());
    }

    private async Task<IReadOnlyList<GuidedDomainTreeResult>> BuildDomainTreeAsync(
        Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var domains = await _dbContext.Domains
            .AsNoTracking()
            .Select(domain => new
            {
                domain.Id,
                domain.Name,
                domain.Description,
                domain.IsActive
            })
            .ToListAsync(cancellationToken);

        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Select(category => new
            {
                category.Id,
                category.DomainId,
                category.ParentCategoryId,
                category.Name,
                category.Description,
                category.DisplayOrder,
                category.IsActive
            })
            .ToListAsync(cancellationToken);

        var programs = await _dbContext.Programs
            .AsNoTracking()
            .Select(program => new
            {
                program.Id,
                program.CategoryId,
                program.ExpertId,
                program.Title,
                program.Description,
                program.ImageUrl,
                program.IsActive,
                program.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var pricing = await _dbContext.ProgramPricing
            .AsNoTracking()
            .Select(price => new
            {
                price.Id,
                price.ProgramId,
                price.DurationWeeks,
                price.OriginalPrice,
                price.DiscountPercentage,
                price.FinalPrice,
                price.CurrencyCode,
                price.IsActive
            })
            .ToListAsync(cancellationToken);

        var durationsByProgram = pricing
            .GroupBy(price => price.ProgramId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<GuidedDurationTreeResult>)group
                    .Select(price => new GuidedDurationTreeResult(
                        price.Id,
                        price.ProgramId,
                        price.DurationWeeks,
                        price.OriginalPrice,
                        price.DiscountPercentage,
                        price.FinalPrice,
                        price.CurrencyCode,
                        price.IsActive))
                    .ToList());

        var programsByCategory = programs
            .GroupBy(program => program.CategoryId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<GuidedProgramTreeResult>)group
                    .Select(program => new GuidedProgramTreeResult(
                        program.Id,
                        program.CategoryId,
                        program.ExpertId,
                        program.Title,
                        program.Description,
                        string.IsNullOrWhiteSpace(program.ImageUrl) ? null : program.ImageUrl,
                        program.IsActive,
                        program.CreatedAt,
                        durationsByProgram.TryGetValue(program.Id, out var durations)
                            ? durations
                            : Array.Empty<GuidedDurationTreeResult>()))
                    .ToList());

        var categoryNodes = categories
            .ToDictionary(
                category => category.Id,
                category => new CategoryNode(
                    category.Id,
                    category.DomainId,
                    category.ParentCategoryId,
                    category.Name,
                    category.Description,
                    category.DisplayOrder,
                    category.IsActive,
                    programsByCategory.TryGetValue(category.Id, out var programsForCategory)
                        ? programsForCategory
                        : Array.Empty<GuidedProgramTreeResult>()));

        foreach (var category in categoryNodes.Values)
        {
            if (category.ParentCategoryId.HasValue
                && categoryNodes.TryGetValue(category.ParentCategoryId.Value, out var parent))
            {
                parent.SubCategories.Add(category);
            }
        }

        Guid? targetCategoryId = null;
        Guid? targetParentCategoryId = null;

        if (categoryId.HasValue)
        {
            var target = categories.FirstOrDefault(category => category.Id == categoryId.Value);
            if (target is null)
            {
                return Array.Empty<GuidedDomainTreeResult>();
            }

            targetCategoryId = target.Id;
            targetParentCategoryId = target.ParentCategoryId;
        }

        return domains
            .Select(domain => new GuidedDomainTreeResult(
                domain.Id,
                domain.Name,
                domain.Description,
                domain.IsActive,
                categoryNodes.Values
                    .Where(category => category.DomainId == domain.Id && category.ParentCategoryId is null)
                    .Where(category => !targetCategoryId.HasValue
                        || (targetParentCategoryId is null && category.CategoryId == targetCategoryId)
                        || (targetParentCategoryId.HasValue && category.CategoryId == targetParentCategoryId))
                    .OrderBy(category => category.DisplayOrder)
                    .Select(category => ToCategoryResult(category, targetCategoryId, targetParentCategoryId))
                    .ToList()))
            .Where(domain => domain.Categories.Count > 0)
            .ToList();
    }

    private static GuidedCategoryTreeResult ToCategoryResult(
        CategoryNode node,
        Guid? targetCategoryId,
        Guid? targetParentCategoryId)
    {
        return new GuidedCategoryTreeResult(
            node.CategoryId,
            node.DomainId,
            node.ParentCategoryId,
            node.Name,
            node.Description,
            node.DisplayOrder,
            node.IsActive,
            node.SubCategories
                .Where(child => !targetCategoryId.HasValue
                    || targetParentCategoryId is null
                    || node.CategoryId != targetParentCategoryId
                    || child.CategoryId == targetCategoryId)
                .OrderBy(child => child.DisplayOrder)
                .Select(child => ToCategoryResult(child, targetCategoryId, targetParentCategoryId))
                .ToList(),
            node.Programs);
    }

    private sealed class CategoryNode
    {
        public CategoryNode(
            Guid categoryId,
            short domainId,
            Guid? parentCategoryId,
            string name,
            string? description,
            int displayOrder,
            bool isActive,
            IReadOnlyList<GuidedProgramTreeResult> programs)
        {
            CategoryId = categoryId;
            DomainId = domainId;
            ParentCategoryId = parentCategoryId;
            Name = name;
            Description = description;
            DisplayOrder = displayOrder;
            IsActive = isActive;
            Programs = programs;
        }

        public Guid CategoryId { get; }
        public short DomainId { get; }
        public Guid? ParentCategoryId { get; }
        public string Name { get; }
        public string? Description { get; }
        public int DisplayOrder { get; }
        public bool IsActive { get; }
        public IReadOnlyList<GuidedProgramTreeResult> Programs { get; }
        public List<CategoryNode> SubCategories { get; } = new();
    }
}
