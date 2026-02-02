using FemvedBackend.Api.Contracts.Authentication;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Authentication;

/// <summary>
/// Validates forgot password requests.
/// </summary>
public sealed class ForgotPasswordRequestDtoValidator : AbstractValidator<ForgotPasswordRequestDto>
{
    public ForgotPasswordRequestDtoValidator()
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
