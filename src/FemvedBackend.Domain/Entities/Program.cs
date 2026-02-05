namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a guided program.
/// </summary>
public class Program : Entity
{
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public Guid ExpertId { get; set; }
    public Expert? Expert { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<ProgramPricing> Pricing { get; set; } = new List<ProgramPricing>();
}
