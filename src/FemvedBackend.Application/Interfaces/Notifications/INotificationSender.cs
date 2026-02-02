using FemvedBackend.Application.Notifications;

namespace FemvedBackend.Application.Interfaces.Notifications;

/// <summary>
/// Sends notifications to external channels.
/// </summary>
public interface INotificationSender
{
    Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
}
