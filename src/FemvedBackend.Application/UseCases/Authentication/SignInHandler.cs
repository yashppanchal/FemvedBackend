using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Handles user sign-in.
/// </summary>
public sealed class SignInHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ILogger<SignInHandler> _logger;

    public SignInHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        ILogger<SignInHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<SignInResult> HandleAsync(SignInRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sign in attempt for {Email}", request.Email);

        var user = await _userRepository.GetByEmailWithRoleAsync(request.Email, cancellationToken);
        if (user is null)
        {
            _logger.LogWarning("Sign in failed for {Email}: user not found", request.Email);
            throw new ValidationException("credentials", "Invalid credentials.");
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Sign in failed for {Email}: invalid password", request.Email);
            throw new ValidationException("credentials", "Invalid credentials.");
        }

        var roles = user.Role is null ? Array.Empty<string>() : new[] { user.Role.Name };

        var tokens = await _tokenService.GenerateTokensAsync(
            user.Id,
            user.Email,
            roles,
            user.CountryCode,
            user.MobileNumber,
            cancellationToken);

        _logger.LogInformation("Sign in succeeded for {UserId}", user.Id);

        return new SignInResult(user.Id, user.Email, tokens);
    }
}
