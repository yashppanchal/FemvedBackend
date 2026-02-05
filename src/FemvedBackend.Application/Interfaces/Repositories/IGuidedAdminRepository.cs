using FemvedBackend.Domain.Entities;
using FemvedBackend.Domain.Entities; // Ensure Category entity type is available

namespace FemvedBackend.Application.Interfaces.Repositories;

/// <summary>
/// Provides admin persistence operations for guided data.
/// </summary>
public interface IGuidedAdminRepository
{
    Task<bool> DomainNameExistsAsync(string name, short? excludeDomainId, CancellationToken cancellationToken = default);
    Task<global::FemvedBackend.Domain.Entities.Domain?> GetDomainByIdAsync(short domainId, CancellationToken cancellationToken = default);
    Task<bool> DomainExistsAsync(short domainId, CancellationToken cancellationToken = default);
    Task AddDomainAsync(global::FemvedBackend.Domain.Entities.Domain domain, CancellationToken cancellationToken = default);
    Task UpdateDomainAsync(global::FemvedBackend.Domain.Entities.Domain domain, CancellationToken cancellationToken = default);
    Task DeleteDomainAsync(short domainId, CancellationToken cancellationToken = default);
    Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
    Task<bool> CategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task AddProgramWithPricingAsync(
        global::FemvedBackend.Domain.Entities.Program program,
        IReadOnlyList<ProgramPricing> pricing,
        CancellationToken cancellationToken = default);
    Task<global::FemvedBackend.Domain.Entities.Program?> GetProgramByIdAsync(Guid programId, CancellationToken cancellationToken = default);
    Task UpdateProgramAsync(global::FemvedBackend.Domain.Entities.Program program, CancellationToken cancellationToken = default);
    Task<bool> ProgramExistsAsync(Guid programId, CancellationToken cancellationToken = default);
    Task DeleteProgramAsync(Guid programId, CancellationToken cancellationToken = default);
    Task AddProgramPricingAsync(ProgramPricing pricing, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProgramPricing>> GetProgramPricingByProgramIdAsync(
        Guid programId,
        CancellationToken cancellationToken = default);
    Task<ProgramPricing?> GetProgramPricingByIdAsync(Guid pricingId, CancellationToken cancellationToken = default);
    Task UpdateProgramPricingAsync(ProgramPricing pricing, CancellationToken cancellationToken = default);
    Task<bool> ProgramPricingDurationExistsAsync(
        Guid programId,
        short durationWeeks,
        Guid? excludePricingId,
        CancellationToken cancellationToken = default);
    Task AddExpertAsync(Expert expert, CancellationToken cancellationToken = default);
    Task<Expert?> GetExpertByIdAsync(Guid expertId, CancellationToken cancellationToken = default);
    Task UpdateExpertAsync(Expert expert, CancellationToken cancellationToken = default);
    Task<bool> ExpertHasProgramsAsync(Guid expertId, CancellationToken cancellationToken = default);
}
