namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a guided domain.
/// </summary>
public class Domain
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
