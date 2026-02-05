namespace FemvedBackend.Api.Contracts.Products;

/// <summary>
/// Represents a product response payload.
/// </summary>
public sealed record ProductResponseDto(
    Guid ProductId,
    short ProductTypeId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive);
