using FemvedBackend.Api.Contracts.Authentication;
using FemvedBackend.Application.Identity;
using FemvedBackend.Application.UseCases.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FemvedBackend.Api.Controllers;

/// <summary>
/// Exposes authentication endpoints.
/// </summary>
[ApiController]
[Route("auth")]
public sealed class AuthController : ControllerBase
{
    private readonly SignUpHandler _signUpHandler;
    private readonly SignInHandler _signInHandler;
    private readonly RefreshTokenHandler _refreshTokenHandler;
    private readonly ForgotPasswordHandler _forgotPasswordHandler;
    private readonly ResetPasswordHandler _resetPasswordHandler;

    public AuthController(
        SignUpHandler signUpHandler,
        SignInHandler signInHandler,
        RefreshTokenHandler refreshTokenHandler,
        ForgotPasswordHandler forgotPasswordHandler,
        ResetPasswordHandler resetPasswordHandler)
    {
        _signUpHandler = signUpHandler;
        _signInHandler = signInHandler;
        _refreshTokenHandler = refreshTokenHandler;
        _forgotPasswordHandler = forgotPasswordHandler;
        _resetPasswordHandler = resetPasswordHandler;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RegisterResponseDto>> Register( [FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var useCaseRequest = new SignUpRequest(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.CountryCode,
            request.MobileNumber);

        var result = await _signUpHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new RegisterResponseDto(
            result.UserId,
            result.Email,
            MapTokens(result.Tokens));

        return Ok(response);
    }

    /// <summary>
    /// Authenticates a user.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new SignInRequest(request.Email, request.Password);
        var result = await _signInHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new LoginResponseDto(
            result.UserId,
            result.Email,
            MapTokens(result.Tokens));

        return Ok(response);
    }

    /// <summary>
    /// Refreshes authentication tokens.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RefreshTokenResponseDto>> Refresh(
        [FromBody] RefreshTokenRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new RefreshTokenRequest(request.RefreshToken);
        var result = await _refreshTokenHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new RefreshTokenResponseDto(MapTokens(result.Tokens));

        return Ok(response);
    }

    /// <summary>
    /// Starts the forgot password flow.
    /// </summary>
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ForgotPasswordResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ForgotPasswordResponseDto>> ForgotPassword(
        [FromBody] ForgotPasswordRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new ForgotPasswordRequest(request.Email, request.ResetLinkBase);
        var result = await _forgotPasswordHandler.HandleAsync(useCaseRequest, cancellationToken);

        return Ok(new ForgotPasswordResponseDto(result.EmailSent));
    }

    /// <summary>
    /// Resets a password using a reset token.
    /// </summary>
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ResetPasswordResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ResetPasswordResponseDto>> ResetPassword(
        [FromBody] ResetPasswordRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new ResetPasswordRequest(request.Email, request.ResetToken, request.NewPassword);
        var result = await _resetPasswordHandler.HandleAsync(useCaseRequest, cancellationToken);

        return Ok(new ResetPasswordResponseDto(result.UserId, result.Email));
    }

    private static AuthTokenResponseDto MapTokens(AuthTokenResult tokens)
        => new(tokens.AccessToken, tokens.AccessTokenExpiresAt, tokens.RefreshToken, tokens.RefreshTokenExpiresAt);
}
