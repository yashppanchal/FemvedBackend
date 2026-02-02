using FemvedBackend.Api.Contracts.Authentication;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Authentication;

/// <summary>
/// Validates login requests.
/// </summary>
public sealed class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
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
