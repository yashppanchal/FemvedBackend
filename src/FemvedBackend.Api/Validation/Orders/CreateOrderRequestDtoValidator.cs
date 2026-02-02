using FemvedBackend.Api.Contracts.Orders;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Orders;

/// <summary>
/// Validates order creation requests.
/// </summary>
public sealed class CreateOrderRequestDtoValidator : AbstractValidator<CreateOrderRequestDto>
{
    public CreateOrderRequestDtoValidator()
    {
        RuleFor(request => request.Items)
            .NotEmpty()
            .WithMessage("At least one order item is required.");

        RuleForEach(request => request.Items)
            .SetValidator(new CreateOrderItemDtoValidator());
    }
}
