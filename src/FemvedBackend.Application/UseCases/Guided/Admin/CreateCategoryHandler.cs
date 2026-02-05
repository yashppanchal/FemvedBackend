using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles category creation.
/// </summary>
public sealed class CreateCategoryHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<CreateCategoryHandler> _logger;

    public CreateCategoryHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<CreateCategoryHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task<CreateCategoryResult> HandleAsync(
        CreateCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating guided category for domain {DomainId}", request.DomainId);
        var domainExists = await _guidedAdminRepository.DomainExistsAsync(request.DomainId, cancellationToken);
        if (!domainExists)
        {
            throw new ValidationException("domainId", "Domain not found.");
        }

        if (request.ParentCategoryId.HasValue)
        {
            var parentCategory = await _guidedAdminRepository.GetCategoryByIdAsync(
                request.ParentCategoryId.Value,
                cancellationToken);

            if (parentCategory is null || parentCategory.DomainId != request.DomainId)
            {
                throw new ValidationException("parentCategoryId", "Parent category must belong to the domain.");
            }
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            DomainId = request.DomainId,
            ParentCategoryId = request.ParentCategoryId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            DisplayOrder = request.DisplayOrder,
            IsActive = request.IsActive
        };

        await _guidedAdminRepository.AddCategoryAsync(category, cancellationToken);
        _logger.LogInformation("Created guided category {CategoryId}", category.Id);

        return new CreateCategoryResult(
            category.Id,
            category.DomainId,
            category.ParentCategoryId,
            category.Name,
            category.Description,
            category.DisplayOrder,
            category.IsActive);
    }
}
