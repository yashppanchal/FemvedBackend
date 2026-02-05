using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Repositories;

namespace FemvedBackend.Application.UseCases.Products;

/// <summary>
/// Handles product deletion for experts.
/// </summary>
public sealed class DeleteProductHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteProductHandler(IProductRepository productRepository, ICurrentUserService currentUserService)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
    }

    public async Task HandleAsync(DeleteProductRequest request, CancellationToken cancellationToken = default)
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

        product.IsActive = false;

        await _productRepository.UpdateAsync(product, cancellationToken);
    }
}
