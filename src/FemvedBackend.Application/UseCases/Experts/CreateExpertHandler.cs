using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
namespace FemvedBackend.Application.UseCases.Experts;

/// <summary>
/// Handles creation of expert profiles.
/// </summary>
public sealed class CreateExpertHandler
{
    private readonly IExpertRepository _expertRepository;

    public CreateExpertHandler(IExpertRepository expertRepository)
    {
        _expertRepository = expertRepository;
    }

    public async Task<CreateExpertResult> HandleAsync(
        CreateExpertRequest request,
        CancellationToken cancellationToken = default)
    {
        var expert = new Expert
        {
            UserId = request.UserId,
            Bio = request.Bio?.Trim() ?? string.Empty,
            Specialization = request.Specialization?.Trim() ?? string.Empty,
            Rating = 0m,
            IsVerified = false
        };

        await _expertRepository.AddAsync(expert, cancellationToken);

        return new CreateExpertResult(expert.UserId, expert.Bio, expert.Specialization, expert.Rating, expert.IsVerified);
    }
}
