using FemvedBackend.Application.UseCases.Authentication;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Authentication;

/// <summary>
/// Validates reset password requests.
/// </summary>
public sealed class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(request => request.ResetToken)
            .NotEmpty()
            .WithMessage("Reset token is required.");

        RuleFor(request => request.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.");
    }
}
