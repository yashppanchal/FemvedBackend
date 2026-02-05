using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles category updates.
/// </summary>
public sealed class UpdateCategoryHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<UpdateCategoryHandler> _logger;

    public UpdateCategoryHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<UpdateCategoryHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task<UpdateCategoryResult> HandleAsync(
        UpdateCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating guided category {CategoryId}", request.CategoryId);
        var category = await _guidedAdminRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new ValidationException("category", "Category not found.");
        }

        category.Name = request.Name.Trim();
        category.Description = request.Description?.Trim() ?? string.Empty;
        category.DisplayOrder = request.DisplayOrder;
        category.IsActive = request.IsActive;

        await _guidedAdminRepository.UpdateCategoryAsync(category, cancellationToken);
        _logger.LogInformation("Updated guided category {CategoryId}", category.Id);

        return new UpdateCategoryResult(
            category.Id,
            category.DomainId,
            category.ParentCategoryId,
            category.Name,
            category.Description,
            category.DisplayOrder,
            category.IsActive);
    }
}
