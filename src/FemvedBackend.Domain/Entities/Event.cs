namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a scheduled event.
/// </summary>
public class Event : Entity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid ExpertId { get; set; }
    public Expert? Expert { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset StartsAt { get; set; }
    public DateTimeOffset EndsAt { get; set; }
}
