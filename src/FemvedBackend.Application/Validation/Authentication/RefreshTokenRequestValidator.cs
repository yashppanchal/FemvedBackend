using FemvedBackend.Application.UseCases.Authentication;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Authentication;

/// <summary>
/// Validates refresh token requests.
/// </summary>
public sealed class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
