namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to update a program.
/// </summary>
public sealed record UpdateProgramRequest(
    Guid ProgramId,
    Guid CategoryId,
    Guid ExpertId,
    string Title,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt);
