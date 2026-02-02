using FemvedBackend.Api.Contracts.Authentication;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Authentication;

/// <summary>
/// Validates refresh token requests.
/// </summary>
public sealed class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestDtoValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
