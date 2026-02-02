using FemvedBackend.Application.UseCases.Payments;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Payments;

public sealed class InitiatePaymentRequestValidator : AbstractValidator<InitiatePaymentRequest>
{
    public InitiatePaymentRequestValidator()
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
