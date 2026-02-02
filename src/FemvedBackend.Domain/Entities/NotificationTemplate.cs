using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a template used to compose notifications.
/// </summary>
public class NotificationTemplate : Entity
{
    public string Name { get; set; } = string.Empty;
    public string TitleTemplate { get; set; } = string.Empty;
    public string BodyTemplate { get; set; } = string.Empty;
    public NotificationChannel Channel { get; set; }
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
