namespace FemvedBackend.Application.UseCases.Access;

/// <summary>
/// Represents a request to grant a user access to a product.
/// </summary>
public sealed record GrantUserAccessRequest(
    Guid UserId,
    Guid ProductId,
    DateTimeOffset? ExpiresAt);
