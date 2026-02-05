namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin category response.
/// </summary>
public sealed record AdminCategoryResponseDto(
    Guid Id,
    short DomainId,
    Guid? ParentCategoryId,
    string Name,
    string? Description,
    int DisplayOrder,
    bool IsActive);
