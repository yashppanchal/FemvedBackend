namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Represents a request to delete program pricing.
/// </summary>
public sealed record DeleteProgramPricingRequest(
    Guid ProgramId,
    Guid PricingId);
