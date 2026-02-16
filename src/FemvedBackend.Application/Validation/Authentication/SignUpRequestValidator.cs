using FemvedBackend.Application.UseCases.Authentication;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Authentication;

/// <summary>
/// Validates sign-up requests.
/// </summary>
public sealed class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required.");

        RuleFor(request => request.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.");

        RuleFor(request => request.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");

        RuleFor(request => request.CountryCode)
            .NotEmpty()
            .WithMessage("Country code is required.")
            .Matches("^\\+\\d{1,4}$")
            .WithMessage("Country code must be in the format +<digits>.");

        RuleFor(request => request.MobileNumber)
            .NotEmpty()
            .WithMessage("Mobile number is required.")
            .Matches("^\\d{7,15}$")
            .WithMessage("Mobile number must contain 7 to 15 digits.");
    }
}
