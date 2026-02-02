using FemvedBackend.Application.Interfaces.Identity;
using Microsoft.Extensions.Logging;
namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Handles refresh token operations.
/// </summary>
public sealed class RefreshTokenHandler
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(ITokenService tokenService, ILogger<RefreshTokenHandler> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<RefreshTokenResult> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Refresh token requested");

        var tokens = await _tokenService.RefreshAsync(request.RefreshToken, cancellationToken);

        _logger.LogInformation("Refresh token succeeded");

        return new RefreshTokenResult(tokens);
    }
}
