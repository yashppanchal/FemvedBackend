using FemvedBackend.Application.UseCases.Orders;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Orders;

public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(request => request.Items)
            .NotEmpty()
            .WithMessage("At least one order item is required.");

        RuleForEach(request => request.Items)
            .SetValidator(new CreateOrderItemRequestValidator());
    }
}
