using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Notifications;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
namespace FemvedBackend.Application.UseCases.Authentication;

/// <summary>
/// Handles forgot password requests.
/// </summary>
public sealed class ForgotPasswordHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ForgotPasswordHandler> _logger;

    public ForgotPasswordHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        IPasswordHasher passwordHasher,
        ILogger<ForgotPasswordHandler> logger)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<ForgotPasswordResult> HandleAsync(
        ForgotPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Forgot password requested for {Email}", request.Email);

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            _logger.LogInformation("Forgot password ignored because user does not exist for {Email}", request.Email);
            return new ForgotPasswordResult(false);
        }

        var token = Guid.NewGuid().ToString("N");
        var hash = _passwordHasher.HashPassword(token);

        user.PasswordResetTokenHash = hash;
        user.PasswordResetTokenExpiresAt = DateTimeOffset.UtcNow.AddHours(1);

        await _userRepository.UpdateAsync(user, cancellationToken);

        var resetLink =
            $"{request.ResetLinkBase}?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

        var subject = "Reset your password";
        var body = $"Use the following link to reset your password: {resetLink}";

        await _emailSender.SendAsync(user.Email, subject, body, cancellationToken);

        _logger.LogInformation("Forgot password email sent for {UserId}", user.Id);

        return new ForgotPasswordResult(true);
    }
}
