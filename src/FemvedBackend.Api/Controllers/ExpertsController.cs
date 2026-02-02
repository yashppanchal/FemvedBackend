using FemvedBackend.Api.Contracts.Experts;
using FemvedBackend.Application.Identity;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.UseCases.Experts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FemvedBackend.Api.Controllers;

/// <summary>
/// Exposes expert-related endpoints.
/// </summary>
[ApiController]
[Route("api/experts")]
[Authorize(Policy = PolicyNames.ExpertOnly)]
public sealed class ExpertsController : ControllerBase
{
    private readonly IExpertRepository _expertRepository;
    private readonly CreateExpertHandler _createExpertHandler;

    public ExpertsController(IExpertRepository expertRepository, CreateExpertHandler createExpertHandler)
    {
        _expertRepository = expertRepository;
        _createExpertHandler = createExpertHandler;
    }

    /// <summary>
    /// Retrieves all experts.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ExpertResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ExpertResponseDto>>> GetExperts(CancellationToken cancellationToken)
    {
        var experts = await _expertRepository.ListAsync(cancellationToken);
        var response = experts
            .Select(expert => new ExpertResponseDto(expert.Id, expert.UserId, expert.DisplayName, expert.Bio))
            .ToList();

        return Ok(response);
    }

    /// <summary>
    /// Retrieves an expert by identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ExpertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpertResponseDto>> GetExpert(Guid id, CancellationToken cancellationToken)
    {
        var expert = await _expertRepository.GetByIdAsync(id, cancellationToken);
        if (expert is null)
        {
            return NotFound();
        }

        var response = new ExpertResponseDto(expert.Id, expert.UserId, expert.DisplayName, expert.Bio);
        return Ok(response);
    }

    /// <summary>
    /// Creates a new expert.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ExpertResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExpertResponseDto>> CreateExpert(
        [FromBody] CreateExpertRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new CreateExpertRequest(request.UserId, request.DisplayName, request.Bio);
        var result = await _createExpertHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new ExpertResponseDto(result.ExpertId, result.UserId, result.DisplayName, result.Bio);
        return CreatedAtAction(nameof(GetExpert), new { id = result.ExpertId }, response);
    }
}
