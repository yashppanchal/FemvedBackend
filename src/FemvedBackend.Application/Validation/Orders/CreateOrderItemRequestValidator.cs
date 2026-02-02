using FemvedBackend.Application.UseCases.Orders;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Orders;

public sealed class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
{
    public CreateOrderItemRequestValidator()
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
