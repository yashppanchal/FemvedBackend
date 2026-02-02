namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents access granted to a user for a product.
/// </summary>
public class UserProductAccess : Entity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTimeOffset GrantedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
}
