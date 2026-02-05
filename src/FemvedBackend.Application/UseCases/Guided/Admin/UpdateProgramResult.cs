namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents the result of updating a program.
/// </summary>
public sealed record UpdateProgramResult(
    Guid Id,
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<UpdateProgramPricingResult> Pricing);
