using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles program pricing updates.
/// </summary>
public sealed class UpdateProgramPricingHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<UpdateProgramPricingHandler> _logger;

    public UpdateProgramPricingHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<UpdateProgramPricingHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task<UpdateProgramPricingResult> HandleAsync(
        UpdateProgramPricingRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating guided program pricing {PricingId} for program {ProgramId}", request.PricingId, request.ProgramId);
        var pricing = await _guidedAdminRepository.GetProgramPricingByIdAsync(request.PricingId, cancellationToken);
        if (pricing is null || pricing.ProgramId != request.ProgramId)
        {
            throw new ValidationException("pricing", "Program pricing not found.");
        }

        var durationExists = await _guidedAdminRepository.ProgramPricingDurationExistsAsync(
            request.ProgramId,
            request.DurationWeeks,
            request.PricingId,
            cancellationToken);

        if (durationExists)
        {
            throw new ValidationException("durationWeeks", "Pricing weeks must be unique per program.");
        }

        pricing.DurationWeeks = request.DurationWeeks;
        pricing.OriginalPrice = request.OriginalPrice;
        pricing.DiscountPercentage = request.DiscountPercentage;
        pricing.FinalPrice = request.FinalPrice;
        pricing.CurrencyCode = request.CurrencyCode;
        pricing.IsActive = request.IsActive;

        await _guidedAdminRepository.UpdateProgramPricingAsync(pricing, cancellationToken);
        _logger.LogInformation("Updated guided program pricing {PricingId}", pricing.Id);

        return new UpdateProgramPricingResult(
            pricing.Id,
            pricing.ProgramId,
            pricing.DurationWeeks,
            pricing.OriginalPrice,
            pricing.DiscountPercentage,
            pricing.FinalPrice,
            pricing.CurrencyCode,
            pricing.IsActive);
    }
}
