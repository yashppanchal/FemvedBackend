using FemvedBackend.Api.Contracts.Experts;
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
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<ExpertProgramsResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ExpertProgramsResponseDto>>> GetExperts(CancellationToken cancellationToken)
    {
        var experts = await _expertRepository.ListActiveWithProgramsAsync(cancellationToken);
        var response = experts
            .Select(MapExpertPrograms)
            .ToList();

        return Ok(response);
    }

    /// <summary>
    /// Retrieves an expert by identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ExpertProgramsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpertProgramsResponseDto>> GetExpert(Guid id, CancellationToken cancellationToken)
    {
        var expert = await _expertRepository.GetActiveWithProgramsAsync(id, cancellationToken);
        if (expert is null)
        {
            return NotFound();
        }

        var response = MapExpertPrograms(expert);
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
        var useCaseRequest = new CreateExpertRequest(request.UserId, request.Bio, request.Specialization);
        var result = await _createExpertHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new ExpertResponseDto(result.UserId, result.Bio, result.Specialization, result.Rating, result.IsVerified);
        return CreatedAtAction(nameof(GetExpert), new { id = result.UserId }, response);
    }

    private static ExpertProgramsResponseDto MapExpertPrograms(ExpertProgramsResult expert)
    {
        var programs = expert.Programs
            .Select(program => new ExpertProgramDto(
                program.ProgramId,
                program.Title,
                program.Description,
                program.ImageUrl,
                program.Durations
                    .Select(duration => new ExpertProgramDurationDto(
                        duration.Weeks,
                        duration.Price,
                        duration.Discount,
                        duration.FinalPrice))
                    .ToList()))
            .ToList();

        return new ExpertProgramsResponseDto(
            expert.ExpertId,
            expert.ExpertName,
            expert.Bio,
            programs);
    }
}
