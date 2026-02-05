using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles program pricing deletion.
/// </summary>
public sealed class DeleteProgramPricingHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<DeleteProgramPricingHandler> _logger;

    public DeleteProgramPricingHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<DeleteProgramPricingHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task HandleAsync(DeleteProgramPricingRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting guided program pricing {PricingId} for program {ProgramId}", request.PricingId, request.ProgramId);
        var pricing = await _guidedAdminRepository.GetProgramPricingByIdAsync(request.PricingId, cancellationToken);
        if (pricing is null || pricing.ProgramId != request.ProgramId)
        {
            throw new ValidationException("pricing", "Program pricing not found.");
        }

        pricing.IsActive = false;

        await _guidedAdminRepository.UpdateProgramPricingAsync(pricing, cancellationToken);
        _logger.LogInformation("Deleted guided program pricing {PricingId}", pricing.Id);
    }
}
