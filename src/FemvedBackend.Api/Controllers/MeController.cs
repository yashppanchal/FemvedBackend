using FemvedBackend.Api.Contracts.Profile;
using FemvedBackend.Application.Identity;
using FemvedBackend.Application.UseCases.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FemvedBackend.Api.Controllers;

/// <summary>
/// Exposes current user profile endpoints.
/// </summary>
[ApiController]
[Route("me")]
public sealed class MeController : ControllerBase
{
    private readonly GetProfileHandler _getProfileHandler;

    public MeController(GetProfileHandler getProfileHandler)
    {
        _getProfileHandler = getProfileHandler;
    }

    /// <summary>
    /// Gets the current user's profile.
    /// </summary>
    [Authorize(Policy = PolicyNames.UserOnly)]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ProfileResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProfileResponseDto>> GetProfile(CancellationToken cancellationToken)
    {
        var result = await _getProfileHandler.HandleAsync(new GetProfileRequest(), cancellationToken);

        var response = new ProfileResponseDto(
            result.UserId,
            result.Email,
            result.FirstName,
            result.LastName,
            result.Country,
            result.Currency,
            result.Role);

        return Ok(response);
    }
}
