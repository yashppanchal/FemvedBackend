using FemvedBackend.Application.UseCases.Authentication;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Authentication;

/// <summary>
/// Validates sign-in requests.
/// </summary>
public sealed class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}
