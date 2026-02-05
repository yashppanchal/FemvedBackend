using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles program pricing creation.
/// </summary>
public sealed class CreateProgramPricingHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<CreateProgramPricingHandler> _logger;

    public CreateProgramPricingHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<CreateProgramPricingHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

public async Task<CreateProgramPricingEntryResult> HandleAsync(
    CreateProgramPricingEntryRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating guided program pricing for program {ProgramId}", request.ProgramId);
        var exists = await _guidedAdminRepository.ProgramExistsAsync(request.ProgramId, cancellationToken);
        if (!exists)
        {
            throw new ValidationException("program", "Program not found.");
        }

        var durationExists = await _guidedAdminRepository.ProgramPricingDurationExistsAsync(
            request.ProgramId,
            request.DurationWeeks,
            null,
            cancellationToken);

        if (durationExists)
        {
            throw new ValidationException("durationWeeks", "Pricing weeks must be unique per program.");
        }

        var pricing = new ProgramPricing
        {
            Id = Guid.NewGuid(),
            ProgramId = request.ProgramId,
            DurationWeeks = request.DurationWeeks,
            OriginalPrice = request.OriginalPrice,
            DiscountPercentage = request.DiscountPercentage,
            FinalPrice = request.FinalPrice,
            CurrencyCode = request.CurrencyCode,
            IsActive = request.IsActive
        };

        await _guidedAdminRepository.AddProgramPricingAsync(pricing, cancellationToken);
        _logger.LogInformation("Created guided program pricing {PricingId}", pricing.Id);

    return new CreateProgramPricingEntryResult(
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
