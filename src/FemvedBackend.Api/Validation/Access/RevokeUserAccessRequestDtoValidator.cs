using FemvedBackend.Api.Contracts.Access;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Access;

/// <summary>
/// Validates revoke access requests.
/// </summary>
public sealed class RevokeUserAccessRequestDtoValidator : AbstractValidator<RevokeUserAccessRequestDto>
{
    public RevokeUserAccessRequestDtoValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(request => request.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");
    }
}
