using FemvedBackend.Api.Contracts.Payments;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Payments;

/// <summary>
/// Validates initiate payment requests.
/// </summary>
public sealed class InitiatePaymentRequestDtoValidator : AbstractValidator<InitiatePaymentRequestDto>
{
    public InitiatePaymentRequestDtoValidator()
    {
        RuleFor(request => request.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required.");

        RuleFor(request => request.PaymentGatewayId)
            .NotEmpty()
            .WithMessage("Payment gateway id is required.");

        RuleFor(request => request.PaymentGatewayName)
            .NotEmpty()
            .WithMessage("Payment gateway name is required.");

        RuleFor(request => request.Currency)
            .NotEmpty()
            .WithMessage("Currency is required.");
    }
}
