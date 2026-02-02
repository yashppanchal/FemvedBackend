using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Application.Notifications;

/// <summary>
/// Represents a notification payload.
/// </summary>
public sealed record NotificationMessage(
    Guid UserId,
    NotificationChannel Channel,
    string Title,
    string Body,
    string? TemplateName);
