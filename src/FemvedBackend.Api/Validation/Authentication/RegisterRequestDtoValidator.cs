using FemvedBackend.Api.Contracts.Authentication;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Authentication;

/// <summary>
/// Validates register requests.
/// </summary>
public sealed class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
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

        RuleFor(request => request.Country)
            .NotEmpty()
            .WithMessage("Country is required.");

        RuleFor(request => request.Currency)
            .NotEmpty()
            .WithMessage("Currency is required.");
    }
}
