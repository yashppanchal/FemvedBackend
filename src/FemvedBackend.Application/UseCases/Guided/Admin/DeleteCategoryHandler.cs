using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles category deletion.
/// </summary>
public sealed class DeleteCategoryHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<DeleteCategoryHandler> _logger;

    public DeleteCategoryHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<DeleteCategoryHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task HandleAsync(DeleteCategoryRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting guided category {CategoryId}", request.CategoryId);
        var exists = await _guidedAdminRepository.CategoryExistsAsync(request.CategoryId, cancellationToken);
        if (!exists)
        {
            throw new ValidationException("category", "Category not found.");
        }

        await _guidedAdminRepository.DeleteCategoryAsync(request.CategoryId, cancellationToken);
        _logger.LogInformation("Deleted guided category {CategoryId}", request.CategoryId);
    }
}
