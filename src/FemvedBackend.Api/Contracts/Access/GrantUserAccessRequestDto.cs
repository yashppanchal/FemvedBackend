namespace FemvedBackend.Api.Contracts.Access;

/// <summary>
/// Represents a request to grant a user access to a product.
/// </summary>
public sealed record GrantUserAccessRequestDto(
    Guid UserId,
    Guid ProductId,
    DateTimeOffset? ExpiresAt);
