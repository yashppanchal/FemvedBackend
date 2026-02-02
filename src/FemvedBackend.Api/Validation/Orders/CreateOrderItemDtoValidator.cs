using FemvedBackend.Api.Contracts.Orders;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Orders;

/// <summary>
/// Validates order item payloads.
/// </summary>
public sealed class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemDtoValidator()
    {
        RuleFor(item => item.ProductVariantId)
            .NotEmpty()
            .WithMessage("Product variant id is required.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than 0.");
    }
}
