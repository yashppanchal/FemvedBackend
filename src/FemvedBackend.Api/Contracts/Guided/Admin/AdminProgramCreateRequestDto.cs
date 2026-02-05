namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin request to create a program with pricing.
/// </summary>
public sealed record AdminProgramCreateRequestDto(
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<AdminProgramPricingRequestDto> Pricing);
