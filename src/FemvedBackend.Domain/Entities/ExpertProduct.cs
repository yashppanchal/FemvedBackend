namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Associates experts with products they deliver.
/// </summary>
public class ExpertProduct : Entity
{
    public Guid ExpertId { get; set; }
    public Expert? Expert { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public short DurationWeeks { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal FinalPrice { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}
