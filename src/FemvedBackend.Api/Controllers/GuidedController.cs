using FemvedBackend.Api.Contracts.Guided;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.UseCases.Guided;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FemvedBackend.Api.Controllers;

/// <summary>
/// Exposes guided domain endpoints.
/// </summary>
[ApiController]
[Route("api/guided")]
public sealed class GuidedController : ControllerBase
{
    private readonly IGuidedRepository _guidedRepository;

    public GuidedController(IGuidedRepository guidedRepository)
    {
        _guidedRepository = guidedRepository;
    }

    /// <summary>
    /// Retrieves the guided domain tree.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<GuidedDomainDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<GuidedDomainDto>>> GetGuided(CancellationToken cancellationToken)
    {
        var domains = await _guidedRepository.GetDomainTreeAsync(cancellationToken);
        var response = domains.Select(MapDomain).ToList();
        return Ok(response);
    }

    /// <summary>
    /// Retrieves the guided domain tree for a specific subcategory.
    /// </summary>
    [HttpGet("subcategories/{subCategoryId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<GuidedDomainDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<GuidedDomainDto>>> GetGuidedBySubCategory(
        Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var domains = await _guidedRepository.GetDomainTreeBySubCategoryAsync(subCategoryId, cancellationToken);
        var response = domains
            .Select(domain => domain with
            {
                Categories = domain.Categories.Take(1)
                    .Select(category => category with
                    {
                        SubCategories = category.SubCategories.Take(1).ToList()
                    })
                    .ToList()
            })
            .Select(MapDomain)
            .ToList();

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a flat list of guided programs.
    /// </summary>
    [HttpGet("programs")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<GuidedProgramFlatDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<GuidedProgramFlatDto>>> GetGuidedPrograms(
        CancellationToken cancellationToken)
    {
        var programs = await _guidedRepository.GetAllProgramsFlatAsync(cancellationToken);
        var response = programs.Select(MapProgramFlat).ToList();
        return Ok(response);
    }

    /// <summary>
    /// Retrieves guided programs for a specific expert.
    /// </summary>
    [HttpGet("experts/{expertId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GuidedExpertProgramsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GuidedExpertProgramsDto>> GetGuidedExpertPrograms(
        Guid expertId,
        CancellationToken cancellationToken)
    {
        var expert = await _guidedRepository.GetExpertWithProgramsAsync(expertId, cancellationToken);
        if (expert is null)
        {
            return NotFound();
        }

        var response = MapExpertPrograms(expert);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves the guided domain tree for a specific category.
    /// </summary>
    [HttpGet("categories/{categoryId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<GuidedDomainDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<GuidedDomainDto>>> GetGuidedByCategory(
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var domains = await _guidedRepository.GetDomainTreeByCategoryAsync(categoryId, cancellationToken);
        var response = domains
            .Select(domain => domain with
            {
                Categories = domain.Categories.Take(1).ToList()
            })
            .Select(MapDomain)
            .ToList();

        return Ok(response);
    }

    private static GuidedDomainDto MapDomain(GuidedDomainTreeResult domain)
    {
        return new GuidedDomainDto(
            domain.DomainId,
            domain.Name,
            domain.Description,
            domain.IsActive,
            domain.Categories.Select(MapCategory).ToList());
    }

    private static GuidedCategoryDto MapCategory(GuidedCategoryTreeResult category)
    {
        return new GuidedCategoryDto(
            category.CategoryId,
            category.DomainId,
            category.ParentCategoryId,
            category.Name,
            category.Description,
            category.DisplayOrder,
            category.IsActive,
            category.SubCategories.Select(MapCategory).ToList(),
            category.Programs.Select(MapProgram).ToList());
    }

    private static GuidedProgramDto MapProgram(GuidedProgramTreeResult program)
    {
        return new GuidedProgramDto(
            program.ProgramId,
            program.CategoryId,
            program.ExpertId,
            program.Title,
            program.Description,
            program.ImageUrl,
            program.IsActive,
            program.CreatedAt,
            program.Durations.Select(MapDuration).ToList());
    }

    private static GuidedDurationDto MapDuration(GuidedDurationTreeResult duration)
    {
        return new GuidedDurationDto(
            duration.PricingId,
            duration.ProgramId,
            duration.DurationWeeks,
            duration.OriginalPrice,
            duration.DiscountPercentage,
            duration.FinalPrice,
            duration.CurrencyCode,
            duration.IsActive);
    }

    private static GuidedProgramFlatDto MapProgramFlat(GuidedProgramFlatResult program)
    {
        return new GuidedProgramFlatDto(
            program.ProgramId,
            program.CategoryId,
            program.ExpertId,
            program.ExpertBio,
            program.ExpertSpecialization,
            program.ExpertRating,
            program.ExpertIsVerified,
            program.Title,
            program.Description,
            program.ImageUrl,
            program.IsActive,
            program.CreatedAt,
            program.Durations
                .Select(duration => new GuidedDurationFlatDto(
                    duration.PricingId,
                    duration.ProgramId,
                    duration.DurationWeeks,
                    duration.OriginalPrice,
                    duration.DiscountPercentage,
                    duration.FinalPrice,
                    duration.CurrencyCode,
                    duration.IsActive))
                .ToList());
    }

    private static GuidedExpertProgramsDto MapExpertPrograms(GuidedExpertProgramsResult expert)
    {
        return new GuidedExpertProgramsDto(
            expert.ExpertId,
            expert.Bio,
            expert.Specialization,
            expert.Rating,
            expert.IsVerified,
            expert.Programs
                .Select(program => new GuidedExpertProgramDto(
                    program.ProgramId,
                    program.CategoryId,
                    program.ExpertId,
                    program.Title,
                    program.Description,
                    program.ImageUrl,
                    program.IsActive,
                    program.CreatedAt,
                    program.Durations
                        .Select(duration => new GuidedExpertProgramDurationDto(
                            duration.PricingId,
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
}
