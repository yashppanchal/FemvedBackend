namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a role assigned to users.
/// </summary>
public class Role
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}
