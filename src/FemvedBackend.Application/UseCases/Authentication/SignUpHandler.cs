using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.Extensions.Logging;
namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Handles user registration.
/// </summary>
public sealed class SignUpHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ILogger<SignUpHandler> _logger;

    public SignUpHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        ILogger<SignUpHandler> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<SignUpResult> HandleAsync(SignUpRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sign up attempt for {Email} with role {RoleType}", request.Email, request.RoleType);

        var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
        {
            _logger.LogWarning("Sign up rejected because user already exists for {Email}", request.Email);
            throw new ValidationException("email", "User already exists.");
        }

        var role = await _roleRepository.GetByTypeAsync(request.RoleType, cancellationToken)
            ?? throw new ValidationException("role", "Role not found.");

        var hash = _passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = hash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Country = request.Country,
            Currency = request.Currency,
            RoleId = role.Id
        };

        await _userRepository.AddAsync(user, cancellationToken);

        _logger.LogInformation("Sign up succeeded for {UserId}", user.Id);

        var tokens = await _tokenService.GenerateTokensAsync(
            user.Id,
            user.Email,
            new[] { role.Name },
            user.Country,
            user.Currency,
            cancellationToken);

        return new SignUpResult(user.Id, user.Email, tokens);
    }
}
