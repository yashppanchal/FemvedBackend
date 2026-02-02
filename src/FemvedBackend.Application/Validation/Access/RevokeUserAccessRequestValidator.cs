using FemvedBackend.Application.UseCases.Access;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Access;

public sealed class RevokeUserAccessRequestValidator : AbstractValidator<RevokeUserAccessRequest>
{
    public RevokeUserAccessRequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(request => request.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");
    }
}
