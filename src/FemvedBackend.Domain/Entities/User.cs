namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents an application user.
/// </summary>
public class User : Entity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string? PasswordResetTokenHash { get; set; }
    public DateTimeOffset? PasswordResetTokenExpiresAt { get; set; }
    public short RoleId { get; set; }
    public Role? Role { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<UserProductAccess> ProductAccesses { get; set; } = new List<UserProductAccess>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
