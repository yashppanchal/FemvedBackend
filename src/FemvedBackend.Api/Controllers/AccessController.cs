using FemvedBackend.Api.Contracts.Access;
using FemvedBackend.Application.UseCases.Access;
using Microsoft.AspNetCore.Mvc;

namespace FemvedBackend.Api.Controllers;

/// <summary>
/// Exposes user access management endpoints.
/// </summary>
[ApiController]
[Route("api/access")]
public sealed class AccessController : ControllerBase
{
    private readonly GrantUserAccessHandler _grantUserAccessHandler;
    private readonly RevokeUserAccessHandler _revokeUserAccessHandler;

    public AccessController(
        GrantUserAccessHandler grantUserAccessHandler,
        RevokeUserAccessHandler revokeUserAccessHandler)
    {
        _grantUserAccessHandler = grantUserAccessHandler;
        _revokeUserAccessHandler = revokeUserAccessHandler;
    }

    /// <summary>
    /// Grants user access to a product.
    /// </summary>
    [HttpPost("grant")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GrantAccess(
        [FromBody] GrantUserAccessRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new GrantUserAccessRequest(request.UserId, request.ProductId, request.ExpiresAt);
        await _grantUserAccessHandler.HandleAsync(useCaseRequest, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Revokes user access to a product.
    /// </summary>
    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeAccess(
        [FromBody] RevokeUserAccessRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new RevokeUserAccessRequest(request.UserId, request.ProductId);
        await _revokeUserAccessHandler.HandleAsync(useCaseRequest, cancellationToken);

        return NoContent();
    }
}
