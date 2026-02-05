namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a guided category.
/// </summary>
public class Category
{
    public Guid Id { get; set; }
    public short DomainId { get; set; }
    public Domain? Domain { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Program> Programs { get; set; } = new List<Program>();
}
