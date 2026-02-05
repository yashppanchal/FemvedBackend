using FemvedBackend.Application.UseCases.Guided;

namespace FemvedBackend.Application.Interfaces.Repositories;

/// <summary>
/// Provides guided domain tree persistence operations.
/// </summary>
public interface IGuidedRepository
{
    Task<IReadOnlyList<GuidedDomainTreeResult>> GetDomainTreeAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GuidedDomainTreeResult>> GetDomainTreeByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GuidedDomainTreeResult>> GetDomainTreeBySubCategoryAsync(Guid subCategoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GuidedProgramFlatResult>> GetAllProgramsFlatAsync(CancellationToken cancellationToken = default);
    Task<GuidedProgramDetailsResult?> GetProgramDetailsAsync(Guid programId, CancellationToken cancellationToken = default);
    Task<GuidedExpertProgramsResult?> GetExpertWithProgramsAsync(Guid expertId, CancellationToken cancellationToken = default);
}
