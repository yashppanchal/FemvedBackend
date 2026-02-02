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
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            DisplayName = request.DisplayName.Trim(),
            Bio = request.Bio?.Trim() ?? string.Empty
        };

        await _expertRepository.AddAsync(expert, cancellationToken);

        return new CreateExpertResult(expert.Id, expert.UserId, expert.DisplayName, expert.Bio);
    }
}
