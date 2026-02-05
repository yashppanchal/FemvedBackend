namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents pricing for a guided program.
/// </summary>
public class ProgramPricing : Entity
{
    public Guid ProgramId { get; set; }
    public Program? Program { get; set; }
    public short DurationWeeks { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal FinalPrice { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
