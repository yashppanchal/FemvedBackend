using FemvedBackend.Api.Contracts.Payments;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Payments;

/// <summary>
/// Validates verify payment requests.
/// </summary>
public sealed class VerifyPaymentRequestDtoValidator : AbstractValidator<VerifyPaymentRequestDto>
{
    public VerifyPaymentRequestDtoValidator()
    {
        RuleFor(request => request.PaymentId)
            .NotEmpty()
            .WithMessage("PaymentId is required.");
    }
}
