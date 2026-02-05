namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin program response.
/// </summary>
public sealed record AdminProgramResponseDto(
    Guid Id,
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<AdminProgramPricingResponseDto> Pricing);
