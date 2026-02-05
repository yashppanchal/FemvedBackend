using FemvedBackend.Api.Contracts.Guided.Admin;
using FemvedBackend.Application.UseCases.Guided.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FemvedBackend.Api.Controllers;

/// <summary>
/// Exposes admin guided endpoints.
/// </summary>
[ApiController]
[Route("api/admin/guided")]
[Authorize(Roles = "Admin")]
public sealed class GuidedAdminController : ControllerBase
{
    private readonly CreateDomainHandler _createDomainHandler;
    private readonly UpdateDomainHandler _updateDomainHandler;
    private readonly DeleteDomainHandler _deleteDomainHandler;
    private readonly CreateCategoryHandler _createCategoryHandler;
    private readonly UpdateCategoryHandler _updateCategoryHandler;
    private readonly DeleteCategoryHandler _deleteCategoryHandler;
    private readonly CreateProgramHandler _createProgramHandler;
    private readonly UpdateProgramHandler _updateProgramHandler;
    private readonly DeleteProgramHandler _deleteProgramHandler;
    private readonly CreateProgramPricingHandler _createProgramPricingHandler;
    private readonly UpdateProgramPricingHandler _updateProgramPricingHandler;
    private readonly DeleteProgramPricingHandler _deleteProgramPricingHandler;
    private readonly CreateExpertHandler _createExpertHandler;
    private readonly UpdateExpertHandler _updateExpertHandler;
    private readonly DeleteExpertHandler _deleteExpertHandler;

    public GuidedAdminController(
        CreateDomainHandler createDomainHandler,
        UpdateDomainHandler updateDomainHandler,
        DeleteDomainHandler deleteDomainHandler,
        CreateCategoryHandler createCategoryHandler,
        UpdateCategoryHandler updateCategoryHandler,
        DeleteCategoryHandler deleteCategoryHandler,
        CreateProgramHandler createProgramHandler,
        UpdateProgramHandler updateProgramHandler,
        DeleteProgramHandler deleteProgramHandler,
        CreateProgramPricingHandler createProgramPricingHandler,
        UpdateProgramPricingHandler updateProgramPricingHandler,
        DeleteProgramPricingHandler deleteProgramPricingHandler,
        CreateExpertHandler createExpertHandler,
        UpdateExpertHandler updateExpertHandler,
        DeleteExpertHandler deleteExpertHandler)
    {
        _createDomainHandler = createDomainHandler;
        _updateDomainHandler = updateDomainHandler;
        _deleteDomainHandler = deleteDomainHandler;
        _createCategoryHandler = createCategoryHandler;
        _updateCategoryHandler = updateCategoryHandler;
        _deleteCategoryHandler = deleteCategoryHandler;
        _createProgramHandler = createProgramHandler;
        _updateProgramHandler = updateProgramHandler;
        _deleteProgramHandler = deleteProgramHandler;
        _createProgramPricingHandler = createProgramPricingHandler;
        _updateProgramPricingHandler = updateProgramPricingHandler;
        _deleteProgramPricingHandler = deleteProgramPricingHandler;
        _createExpertHandler = createExpertHandler;
        _updateExpertHandler = updateExpertHandler;
        _deleteExpertHandler = deleteExpertHandler;
    }

    /// <summary>
    /// Creates a guided domain.
    /// </summary>
    [HttpPost("domains")]
    [ProducesResponseType(typeof(AdminDomainResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminDomainResponseDto>> CreateDomain(
        [FromBody] AdminDomainRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new CreateDomainRequest(request.Name, request.Description, request.IsActive);
        var result = await _createDomainHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminDomainResponseDto(result.Id, result.Name, result.Description, result.IsActive);
        return CreatedAtAction(nameof(CreateDomain), new { id = result.Id }, response);
    }

    /// <summary>
    /// Updates a guided domain.
    /// </summary>
    [HttpPut("domains/{domainId:short}")]
    [ProducesResponseType(typeof(AdminDomainResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminDomainResponseDto>> UpdateDomain(
        short domainId,
        [FromBody] AdminDomainRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new UpdateDomainRequest(domainId, request.Name, request.Description, request.IsActive);
        var result = await _updateDomainHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminDomainResponseDto(result.Id, result.Name, result.Description, result.IsActive);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a guided domain and its related data.
    /// </summary>
    [HttpDelete("domains/{domainId:short}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDomain(short domainId, CancellationToken cancellationToken)
    {
        var request = new DeleteDomainRequest(domainId);
        await _deleteDomainHandler.HandleAsync(request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Creates a guided category.
    /// </summary>
    [HttpPost("categories")]
    [ProducesResponseType(typeof(AdminCategoryResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminCategoryResponseDto>> CreateCategory(
        [FromBody] AdminCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new CreateCategoryRequest(
            request.DomainId,
            request.ParentCategoryId,
            request.Name,
            request.Description,
            request.DisplayOrder,
            request.IsActive);

        var result = await _createCategoryHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminCategoryResponseDto(
            result.Id,
            result.DomainId,
            result.ParentCategoryId,
            result.Name,
            result.Description,
            result.DisplayOrder,
            result.IsActive);

        return CreatedAtAction(nameof(CreateCategory), new { id = result.Id }, response);
    }

    /// <summary>
    /// Updates a guided category.
    /// </summary>
    [HttpPut("categories/{categoryId:guid}")]
    [ProducesResponseType(typeof(AdminCategoryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminCategoryResponseDto>> UpdateCategory(
        Guid categoryId,
        [FromBody] AdminCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new UpdateCategoryRequest(
            categoryId,
            request.Name,
            request.Description,
            request.DisplayOrder,
            request.IsActive);

        var result = await _updateCategoryHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminCategoryResponseDto(
            result.Id,
            result.DomainId,
            result.ParentCategoryId,
            result.Name,
            result.Description,
            result.DisplayOrder,
            result.IsActive);

        return Ok(response);
    }

    /// <summary>
    /// Deletes a guided category and its related data.
    /// </summary>
    [HttpDelete("categories/{categoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCategory(Guid categoryId, CancellationToken cancellationToken)
    {
        var request = new DeleteCategoryRequest(categoryId);
        await _deleteCategoryHandler.HandleAsync(request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Creates a guided program with pricing.
    /// </summary>
    [HttpPost("programs")]
    [ProducesResponseType(typeof(AdminProgramResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminProgramResponseDto>> CreateProgram(
        [FromBody] AdminProgramCreateRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new CreateProgramRequest(
            request.CategoryId,
            request.ExpertId,
            request.Title,
            request.Description,
            request.ImageUrl,
            request.IsActive,
            request.CreatedAt,
            request.Pricing.Select(item => new CreateProgramPricingRequest(
                item.DurationWeeks,
                item.OriginalPrice,
                item.DiscountPercentage,
                item.FinalPrice,
                item.CurrencyCode,
                item.IsActive))
            .ToList());

        var result = await _createProgramHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminProgramResponseDto(
            result.Id,
            result.CategoryId,
            result.ExpertId,
            result.Title,
            result.Description,
            result.ImageUrl,
            result.IsActive,
            result.CreatedAt,
            result.Pricing.Select(item => new AdminProgramPricingResponseDto(
                item.Id,
                item.ProgramId,
                item.DurationWeeks,
                item.OriginalPrice,
                item.DiscountPercentage,
                item.FinalPrice,
                item.CurrencyCode,
                item.IsActive))
            .ToList());

        return CreatedAtAction(nameof(CreateProgram), new { id = result.Id }, response);
    }

    /// <summary>
    /// Updates a guided program.
    /// </summary>
    [HttpPut("programs/{programId:guid}")]
    [ProducesResponseType(typeof(AdminProgramResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminProgramResponseDto>> UpdateProgram(
        Guid programId,
        [FromBody] AdminProgramRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new UpdateProgramRequest(
            programId,
            request.CategoryId,
            request.ExpertId,
            request.Title,
            request.Description,
            request.ImageUrl,
            request.IsActive,
            request.CreatedAt);

        var result = await _updateProgramHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminProgramResponseDto(
            result.Id,
            result.CategoryId,
            result.ExpertId,
            result.Title,
            result.Description,
            result.ImageUrl,
            result.IsActive,
            result.CreatedAt,
            result.Pricing.Select(item => new AdminProgramPricingResponseDto(
                item.Id,
                item.ProgramId,
                item.DurationWeeks,
                item.OriginalPrice,
                item.DiscountPercentage,
                item.FinalPrice,
                item.CurrencyCode,
                item.IsActive))
            .ToList());

        return Ok(response);
    }

    /// <summary>
    /// Deletes a guided program and its pricing.
    /// </summary>
    [HttpDelete("programs/{programId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProgram(Guid programId, CancellationToken cancellationToken)
    {
        var request = new DeleteProgramRequest(programId);
        await _deleteProgramHandler.HandleAsync(request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Creates pricing for a guided program.
    /// </summary>
    [HttpPost("programs/{programId:guid}/pricing")]
    [ProducesResponseType(typeof(AdminProgramPricingResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminProgramPricingResponseDto>> CreateProgramPricing(
        Guid programId,
        [FromBody] AdminProgramPricingRequestDto request,
        CancellationToken cancellationToken)
    {
        var targetProgramId = programId;
        var useCaseRequest = new CreateProgramPricingEntryRequest(
            targetProgramId,
            request.DurationWeeks,
            request.OriginalPrice,
            request.DiscountPercentage,
            request.FinalPrice,
            request.CurrencyCode,
            request.IsActive);

        var result = await _createProgramPricingHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminProgramPricingResponseDto(
            result.Id,
            result.ProgramId,
            result.DurationWeeks,
            result.OriginalPrice,
            result.DiscountPercentage,
            result.FinalPrice,
            result.CurrencyCode,
            result.IsActive);

        return CreatedAtAction(nameof(CreateProgramPricing), new { id = result.Id }, response);
    }

    /// <summary>
    /// Updates pricing for a guided program.
    /// </summary>
    [HttpPut("programs/{programId:guid}/pricing/{pricingId:guid}")]
    [ProducesResponseType(typeof(AdminProgramPricingResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminProgramPricingResponseDto>> UpdateProgramPricing(
        Guid programId,
        Guid pricingId,
        [FromBody] AdminProgramPricingRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new UpdateProgramPricingRequest(
            pricingId,
            programId,
            request.DurationWeeks,
            request.OriginalPrice,
            request.DiscountPercentage,
            request.FinalPrice,
            request.CurrencyCode,
            request.IsActive);

        var result = await _updateProgramPricingHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminProgramPricingResponseDto(
            result.Id,
            result.ProgramId,
            result.DurationWeeks,
            result.OriginalPrice,
            result.DiscountPercentage,
            result.FinalPrice,
            result.CurrencyCode,
            result.IsActive);

        return Ok(response);
    }

    /// <summary>
    /// Deletes pricing for a guided program.
    /// </summary>
    [HttpDelete("programs/{programId:guid}/pricing/{pricingId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProgramPricing(
        Guid programId,
        Guid pricingId,
        CancellationToken cancellationToken)
    {
        var request = new DeleteProgramPricingRequest(programId, pricingId);
        await _deleteProgramPricingHandler.HandleAsync(request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Creates a guided expert.
    /// </summary>
    [HttpPost("experts")]
    [ProducesResponseType(typeof(AdminExpertResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminExpertResponseDto>> CreateExpert(
        [FromBody] AdminExpertRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new CreateExpertRequest(
            request.UserId,
            request.Bio,
            request.Specialization,
            request.Rating,
            request.IsVerified);

        var result = await _createExpertHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminExpertResponseDto(
            result.UserId,
            result.Bio,
            result.Specialization,
            result.Rating,
            result.IsVerified);

        return CreatedAtAction(nameof(CreateExpert), new { id = result.UserId }, response);
    }

    /// <summary>
    /// Updates a guided expert.
    /// </summary>
    [HttpPut("experts/{expertId:guid}")]
    [ProducesResponseType(typeof(AdminExpertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminExpertResponseDto>> UpdateExpert(
        Guid expertId,
        [FromBody] AdminExpertRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new UpdateExpertRequest(
            expertId,
            request.Bio,
            request.Specialization,
            request.Rating,
            request.IsVerified);

        var result = await _updateExpertHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new AdminExpertResponseDto(
            result.UserId,
            result.Bio,
            result.Specialization,
            result.Rating,
            result.IsVerified);

        return Ok(response);
    }

    /// <summary>
    /// Deletes a guided expert.
    /// </summary>
    [HttpDelete("experts/{expertId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteExpert(Guid expertId, CancellationToken cancellationToken)
    {
        var request = new DeleteExpertRequest(expertId);
        await _deleteExpertHandler.HandleAsync(request, cancellationToken);
        return NoContent();
    }
}
