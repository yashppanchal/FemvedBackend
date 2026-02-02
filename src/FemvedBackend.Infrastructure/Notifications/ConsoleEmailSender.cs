using FemvedBackend.Application.Interfaces.Notifications;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Infrastructure.Notifications;

public sealed class ConsoleEmailSender : IEmailSender
{
    private readonly ILogger<ConsoleEmailSender> _logger;

    public ConsoleEmailSender(ILogger<ConsoleEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending email to {Email}. Subject: {Subject}. Body: {Body}", toEmail, subject, body);
        return Task.CompletedTask;
    }
}
