namespace FemvedBackend.Application.UseCases.Access;

/// <summary>
/// Represents a request to revoke a user's access to a product.
/// </summary>
public sealed record RevokeUserAccessRequest(
    Guid UserId,
    Guid ProductId);
