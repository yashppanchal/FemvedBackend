using FemvedBackend.Application.UseCases.Experts;
using FluentValidation;

namespace FemvedBackend.Application.Validation.Experts;

public sealed class CreateExpertRequestValidator : AbstractValidator<CreateExpertRequest>
{
    public CreateExpertRequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(request => request.DisplayName)
            .NotEmpty()
            .WithMessage("Display name is required.")
            .MaximumLength(150)
            .WithMessage("Display name must be 150 characters or fewer.");
    }
}
