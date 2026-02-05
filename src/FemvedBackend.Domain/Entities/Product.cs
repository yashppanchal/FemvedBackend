namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a sellable product.
/// </summary>
public class Product : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public short ProductTypeId { get; set; }
    public ProductType? ProductType { get; set; }
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<ExpertProduct> ExpertProducts { get; set; } = new List<ExpertProduct>();
    public ICollection<RecordedContent> RecordedContents { get; set; } = new List<RecordedContent>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
