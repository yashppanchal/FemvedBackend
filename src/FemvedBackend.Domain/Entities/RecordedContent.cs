namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents recorded content for a product.
/// </summary>
public class RecordedContent : Entity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ContentUrl { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
}
