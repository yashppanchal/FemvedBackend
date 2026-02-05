using FemvedBackend.Api.Contracts.Products;
using FemvedBackend.Application.Identity;
using FemvedBackend.Application.UseCases.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FemvedBackend.Api.Controllers;

/// <summary>
/// Exposes product management endpoints for experts.
/// </summary>
[ApiController]
[Route("api/products")]
[Authorize(Policy = PolicyNames.ExpertOnly)]
public sealed class ProductsController : ControllerBase
{
    private readonly CreateProductHandler _createProductHandler;
    private readonly UpdateProductHandler _updateProductHandler;
    private readonly DeleteProductHandler _deleteProductHandler;

    public ProductsController(
        CreateProductHandler createProductHandler,
        UpdateProductHandler updateProductHandler,
        DeleteProductHandler deleteProductHandler)
    {
        _createProductHandler = createProductHandler;
        _updateProductHandler = updateProductHandler;
        _deleteProductHandler = deleteProductHandler;
    }

    /// <summary>
    /// Creates a new product for the current expert.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductResponseDto>> CreateProduct(
        [FromBody] CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new CreateProductRequest(
            request.ProductTypeId,
            request.Title,
            request.Description,
            request.ImageUrl);

        var result = await _createProductHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new ProductResponseDto(
            result.ProductId,
            result.ProductTypeId,
            result.Title,
            result.Description,
            result.ImageUrl,
            result.IsActive);

        return CreatedAtAction(nameof(CreateProduct), new { id = result.ProductId }, response);
    }

    /// <summary>
    /// Updates an existing product for the current expert.
    /// </summary>
    [HttpPut("{productId:guid}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductResponseDto>> UpdateProduct(
        Guid productId,
        [FromBody] UpdateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var useCaseRequest = new UpdateProductRequest(
            productId,
            request.ProductTypeId,
            request.Title,
            request.Description,
            request.ImageUrl);

        var result = await _updateProductHandler.HandleAsync(useCaseRequest, cancellationToken);

        var response = new ProductResponseDto(
            result.ProductId,
            result.ProductTypeId,
            result.Title,
            result.Description,
            result.ImageUrl,
            result.IsActive);

        return Ok(response);
    }

    /// <summary>
    /// Soft deletes a product for the current expert.
    /// </summary>
    [HttpDelete("{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduct(Guid productId, CancellationToken cancellationToken)
    {
        var useCaseRequest = new DeleteProductRequest(productId);
        await _deleteProductHandler.HandleAsync(useCaseRequest, cancellationToken);

        return NoContent();
    }
}
