using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles program updates.
/// </summary>
public sealed class UpdateProgramHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<UpdateProgramHandler> _logger;

    public UpdateProgramHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<UpdateProgramHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task<UpdateProgramResult> HandleAsync(
        UpdateProgramRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating guided program {ProgramId}", request.ProgramId);
        var program = await _guidedAdminRepository.GetProgramByIdAsync(request.ProgramId, cancellationToken);
        if (program is null)
        {
            throw new ValidationException("program", "Program not found.");
        }

        program.CategoryId = request.CategoryId;
        program.ExpertId = request.ExpertId;
        program.Title = request.Title.Trim();
        program.Description = request.Description?.Trim() ?? string.Empty;
        program.ImageUrl = request.ImageUrl?.Trim() ?? string.Empty;
        program.IsActive = request.IsActive;
        program.CreatedAt = request.CreatedAt;

        await _guidedAdminRepository.UpdateProgramAsync(program, cancellationToken);
        _logger.LogInformation("Updated guided program {ProgramId}", program.Id);

        var pricing = await _guidedAdminRepository.GetProgramPricingByProgramIdAsync(program.Id, cancellationToken);
        var pricingResults = pricing
            .Select(item => new UpdateProgramPricingResult(
                item.Id,
                item.ProgramId,
                item.DurationWeeks,
                item.OriginalPrice,
                item.DiscountPercentage,
                item.FinalPrice,
                item.CurrencyCode,
                item.IsActive))
            .ToList();

        return new UpdateProgramResult(
            program.Id,
            program.CategoryId,
            program.ExpertId,
            program.Title,
            program.Description,
            string.IsNullOrWhiteSpace(program.ImageUrl) ? null : program.ImageUrl,
            program.IsActive,
            program.CreatedAt,
            pricingResults);
    }
}
