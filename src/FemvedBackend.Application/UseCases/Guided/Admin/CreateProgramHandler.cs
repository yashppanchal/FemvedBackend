using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles program creation.
/// </summary>
public sealed class CreateProgramHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<CreateProgramHandler> _logger;

    public CreateProgramHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<CreateProgramHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task<CreateProgramResult> HandleAsync(
        CreateProgramRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating guided program for category {CategoryId}", request.CategoryId);
        var category = await _guidedAdminRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
        if (category is null || !category.ParentCategoryId.HasValue)
        {
            throw new ValidationException("categoryId", "Program must belong to a subcategory.");
        }

        if (request.Pricing.GroupBy(item => item.DurationWeeks).Any(group => group.Count() > 1))
        {
            throw new ValidationException("durationWeeks", "Pricing weeks must be unique per program.");
        }

        var program = new global::FemvedBackend.Domain.Entities.Program
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            ExpertId = request.ExpertId,
            Title = request.Title.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            ImageUrl = request.ImageUrl?.Trim() ?? string.Empty,
            IsActive = request.IsActive,
            CreatedAt = request.CreatedAt
        };

        var pricing = request.Pricing
            .Select(item => new ProgramPricing
            {
                Id = Guid.NewGuid(),
                ProgramId = program.Id,
                DurationWeeks = item.DurationWeeks,
                OriginalPrice = item.OriginalPrice,
                DiscountPercentage = item.DiscountPercentage,
                FinalPrice = item.FinalPrice,
                CurrencyCode = item.CurrencyCode,
                IsActive = item.IsActive
            })
            .ToList();

        await _guidedAdminRepository.AddProgramWithPricingAsync(program, pricing, cancellationToken);
        _logger.LogInformation("Created guided program {ProgramId}", program.Id);

        return new CreateProgramResult(
            program.Id,
            program.CategoryId,
            program.ExpertId,
            program.Title,
            program.Description,
            string.IsNullOrWhiteSpace(program.ImageUrl) ? null : program.ImageUrl,
            program.IsActive,
            program.CreatedAt,
            pricing.Select(item => new CreateProgramPricingResult(
                item.Id,
                item.ProgramId,
                item.DurationWeeks,
                item.OriginalPrice,
                item.DiscountPercentage,
                item.FinalPrice,
                item.CurrencyCode,
                item.IsActive))
                .ToList());
    }
}
