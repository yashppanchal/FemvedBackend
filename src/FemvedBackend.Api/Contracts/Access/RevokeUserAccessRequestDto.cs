namespace FemvedBackend.Api.Contracts.Access;

/// <summary>
/// Represents a request to revoke a user's access to a product.
/// </summary>
public sealed record RevokeUserAccessRequestDto(
    Guid UserId,
    Guid ProductId);
