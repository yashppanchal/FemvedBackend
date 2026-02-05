namespace FemvedBackend.Api.Contracts.Guided.Admin;

/// <summary>
/// Represents an admin request to create or update a program.
/// </summary>
public sealed record AdminProgramRequestDto(
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt);
