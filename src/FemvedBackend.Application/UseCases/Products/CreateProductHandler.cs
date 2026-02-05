using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;

namespace FemvedBackend.Application.UseCases.Products;

/// <summary>
/// Handles product creation for experts.
/// </summary>
public sealed class CreateProductHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IExpertRepository _expertRepository;

    public CreateProductHandler(
        IProductRepository productRepository,
        ICurrentUserService currentUserService,
        IExpertRepository expertRepository)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
        _expertRepository = expertRepository;
    }

    public async Task<CreateProductResult> HandleAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;
        if (!userId.HasValue)
        {
            throw new ValidationException("user", "User is not authenticated.");
        }

        var isVerified = await _expertRepository.IsVerifiedAsync(userId.Value, cancellationToken);
        if (!isVerified)
        {
            throw new ValidationException("expert", "Expert must be verified to publish products.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            ProductTypeId = request.ProductTypeId,
            Title = request.Title.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            ImageUrl = request.ImageUrl?.Trim() ?? string.Empty,
            CreatedBy = userId.Value,
            IsActive = true
        };

        await _productRepository.AddAsync(product, cancellationToken);

        return new CreateProductResult(
            product.Id,
            product.ProductTypeId,
            product.Title,
            product.Description,
            string.IsNullOrWhiteSpace(product.ImageUrl) ? null : product.ImageUrl,
            product.IsActive);
    }
}
