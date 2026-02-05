namespace FemvedBackend.Api.Contracts.Guided;

/// <summary>
/// Represents a guided domain with nested categories.
/// </summary>
public sealed record GuidedDomainDto(
    short DomainId,
    string Name,
    string? Description,
    bool IsActive,
    IReadOnlyList<GuidedCategoryDto> Categories);
