using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Handles password reset operations.
/// </summary>
public sealed class ResetPasswordHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ResetPasswordHandler> _logger;

    public ResetPasswordHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ILogger<ResetPasswordHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<ResetPasswordResult> HandleAsync(
        ResetPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Reset password attempt for {Email}", request.Email);

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            _logger.LogWarning("Reset password failed for {Email}: user not found", request.Email);
            throw new ValidationException("token", "Invalid reset token.");
        }

        if (user.PasswordResetTokenExpiresAt is null ||
            user.PasswordResetTokenExpiresAt <= DateTimeOffset.UtcNow ||
            string.IsNullOrWhiteSpace(user.PasswordResetTokenHash))
        {
            _logger.LogWarning("Reset password failed for {Email}: token expired or missing", request.Email);
            throw new ValidationException("token", "Invalid reset token.");
        }

        if (!_passwordHasher.VerifyPassword(request.ResetToken, user.PasswordResetTokenHash))
        {
            _logger.LogWarning("Reset password failed for {Email}: token mismatch", request.Email);
            throw new ValidationException("token", "Invalid reset token.");
        }

        var hash = _passwordHasher.HashPassword(request.NewPassword);

        user.PasswordHash = hash;
        user.PasswordResetTokenHash = null;
        user.PasswordResetTokenExpiresAt = null;

        await _userRepository.UpdateAsync(user, cancellationToken);

        _logger.LogInformation("Reset password succeeded for {UserId}", user.Id);

        return new ResetPasswordResult(user.Id, user.Email);
    }
}
