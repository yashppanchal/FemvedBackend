using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles expert creation.
/// </summary>
public sealed class CreateExpertHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<CreateExpertHandler> _logger;

    public CreateExpertHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<CreateExpertHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task<CreateExpertResult> HandleAsync(
        CreateExpertRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating guided expert {ExpertId}", request.UserId);
        var expert = new Expert
        {
            UserId = request.UserId,
            Bio = request.Bio?.Trim() ?? string.Empty,
            Specialization = request.Specialization?.Trim() ?? string.Empty,
            Rating = request.Rating,
            IsVerified = request.IsVerified
        };

        await _guidedAdminRepository.AddExpertAsync(expert, cancellationToken);

        _logger.LogInformation("Created guided expert {ExpertId}", expert.UserId);

        return new CreateExpertResult(
            expert.UserId,
            expert.Bio,
            expert.Specialization,
            expert.Rating,
            expert.IsVerified);
    }
}
