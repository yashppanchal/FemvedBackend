namespace FemvedBackend.Application.Identity;

/// <summary>
/// Represents JWT configuration options.
/// </summary>
public sealed class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; }
    public int RefreshTokenDays { get; set; }
}
