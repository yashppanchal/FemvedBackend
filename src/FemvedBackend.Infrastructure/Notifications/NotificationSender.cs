using FemvedBackend.Application.Interfaces.Notifications;
using FemvedBackend.Application.Notifications;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Infrastructure.Notifications;

public sealed class NotificationSender : INotificationSender
{
    private readonly ILogger<NotificationSender> _logger;

    public NotificationSender(ILogger<NotificationSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending notification to {UserId} via {Channel}", message.UserId, message.Channel);
        return Task.CompletedTask;
    }
}
