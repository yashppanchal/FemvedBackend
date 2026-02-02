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
}
