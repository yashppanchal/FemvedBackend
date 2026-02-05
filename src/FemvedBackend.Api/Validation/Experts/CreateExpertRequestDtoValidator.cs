using FemvedBackend.Api.Contracts.Experts;
using FluentValidation;

namespace FemvedBackend.Api.Validation.Experts;

/// <summary>
/// Validates create expert requests.
/// </summary>
public sealed class CreateExpertRequestDtoValidator : AbstractValidator<CreateExpertRequestDto>
{
    public CreateExpertRequestDtoValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
    }
}
