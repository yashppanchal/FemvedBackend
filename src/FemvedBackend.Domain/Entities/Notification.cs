using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a notification delivered to a user.
/// </summary>
public class Notification : Entity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid NotificationTemplateId { get; set; }
    public NotificationTemplate? NotificationTemplate { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
}
