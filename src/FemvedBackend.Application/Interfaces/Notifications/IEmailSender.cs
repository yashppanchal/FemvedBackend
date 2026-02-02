namespace FemvedBackend.Application.Interfaces.Notifications;

/// <summary>
/// Sends email messages.
/// </summary>
public interface IEmailSender
{
    Task SendAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default);
}
