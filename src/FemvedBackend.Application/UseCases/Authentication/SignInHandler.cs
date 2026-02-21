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
        _logger.LogInformation("Sign in debug: user null? {IsNull}", user is null);
        if (user is null)
        {
            _logger.LogWarning("Sign in failed for {Email}: user not found", request.Email);
            throw new ValidationException("credentials", "Invalid credentials.");
        }

        _logger.LogInformation("Sign in debug: user IsActive? {IsActive}", user.IsActive);
        _logger.LogInformation("Sign in debug: role null? {IsRoleNull}", user.Role is null);

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        _logger.LogInformation("Sign in debug: password verification result {IsPasswordValid}", isPasswordValid);
        if (!isPasswordValid)
        {
            _logger.LogWarning("Sign in failed for {Email}: invalid password", request.Email);
            throw new ValidationException("credentials", "Invalid credentials.");
        }

        if (user.Role is null)
        {
            _logger.LogWarning("Sign in failed for {Email}: role not found", request.Email);
            throw new ValidationException("credentials", "Invalid credentials.");
        }

        var roles = new[] { user.Role.Name };

        var tokens = await _tokenService.GenerateTokensAsync(
            user.Id,
            user.Email,
            roles,
            user.CountryCode,
            user.MobileNumber,
            cancellationToken);

        _logger.LogInformation("Sign in succeeded for {UserId}", user.Id);

        var userResult = new SignInUserResult(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.MobileNumber,
            user.IsEmailVerified,
            user.IsMobileVerified,
            new SignInRoleResult(user.Role.Id, user.Role.Name));

        return new SignInResult(tokens, userResult);
    }
}
