using FemvedBackend.Application.UseCases.Payments;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Payments;

public sealed class VerifyPaymentRequestValidator : AbstractValidator<VerifyPaymentRequest>
{
    public VerifyPaymentRequestValidator()
    {
        RuleFor(request => request.PaymentId)
            .NotEmpty()
            .WithMessage("PaymentId is required.");
    }
}
