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
    }
}
