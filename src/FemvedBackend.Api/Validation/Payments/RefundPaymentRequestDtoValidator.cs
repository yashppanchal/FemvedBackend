using FemvedBackend.Api.Contracts.Payments;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Payments;

/// <summary>
/// Validates refund payment requests.
/// </summary>
public sealed class RefundPaymentRequestDtoValidator : AbstractValidator<RefundPaymentRequestDto>
{
    public RefundPaymentRequestDtoValidator()
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
