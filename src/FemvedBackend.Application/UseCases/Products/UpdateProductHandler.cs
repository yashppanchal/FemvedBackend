using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Repositories;

namespace FemvedBackend.Application.UseCases.Products;

/// <summary>
/// Handles product updates for experts.
/// </summary>
public sealed class UpdateProductHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProductHandler(IProductRepository productRepository, ICurrentUserService currentUserService)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateProductResult> HandleAsync(
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;
        if (!userId.HasValue)
        {
            throw new ValidationException("user", "User is not authenticated.");
        }

        var product = await _productRepository.GetByIdForOwnerAsync(request.ProductId, userId.Value, cancellationToken);
        if (product is null)
        {
            throw new ValidationException("product", "Product not found.");
        }

        product.ProductTypeId = request.ProductTypeId;
        product.Title = request.Title.Trim();
        product.Description = request.Description?.Trim() ?? string.Empty;
        product.ImageUrl = request.ImageUrl?.Trim() ?? string.Empty;

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new UpdateProductResult(
            product.Id,
            product.ProductTypeId,
            product.Title,
            product.Description,
            string.IsNullOrWhiteSpace(product.ImageUrl) ? null : product.ImageUrl,
            product.IsActive);
    }
}
