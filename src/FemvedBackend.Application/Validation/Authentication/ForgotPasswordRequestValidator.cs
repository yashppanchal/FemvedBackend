using FemvedBackend.Application.UseCases.Authentication;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Authentication;

/// <summary>
/// Validates forgot password requests.
/// </summary>
public sealed class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(request => request.ResetLinkBase)
            .NotEmpty()
            .WithMessage("Reset link base is required.");
    }
}
