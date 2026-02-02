using FemvedBackend.Application.UseCases.Payments;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Payments;

public sealed class RefundPaymentRequestValidator : AbstractValidator<RefundPaymentRequest>
{
    public RefundPaymentRequestValidator()
    {
        RuleFor(request => request.PaymentId)
            .NotEmpty()
            .WithMessage("PaymentId is required.");

        RuleFor(request => request.PaymentGatewayName)
            .NotEmpty()
            .WithMessage("Payment gateway name is required.");

        RuleFor(request => request.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0.");
    }
}
