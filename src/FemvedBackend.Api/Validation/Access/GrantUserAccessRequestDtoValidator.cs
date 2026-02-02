using FemvedBackend.Api.Contracts.Access;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Access;

/// <summary>
/// Validates grant access requests.
/// </summary>
public sealed class GrantUserAccessRequestDtoValidator : AbstractValidator<GrantUserAccessRequestDto>
{
    public GrantUserAccessRequestDtoValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(request => request.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");
    }
}
