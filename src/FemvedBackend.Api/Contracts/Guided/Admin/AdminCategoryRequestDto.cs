namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin request to create or update a category.
/// </summary>
public sealed record AdminCategoryRequestDto(
    short DomainId,
    Guid? ParentCategoryId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive);
