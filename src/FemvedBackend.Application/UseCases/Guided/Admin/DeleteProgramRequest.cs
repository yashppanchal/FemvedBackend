namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to delete a program.
/// </summary>
public sealed record DeleteProgramRequest(Guid ProgramId);
