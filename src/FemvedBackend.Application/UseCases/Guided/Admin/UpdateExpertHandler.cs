using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles expert updates.
/// </summary>
public sealed class UpdateExpertHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<UpdateExpertHandler> _logger;

    public UpdateExpertHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<UpdateExpertHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task<UpdateExpertResult> HandleAsync(
        UpdateExpertRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating guided expert {ExpertId}", request.UserId);
        var expert = await _guidedAdminRepository.GetExpertByIdAsync(request.UserId, cancellationToken);
        if (expert is null)
        {
            throw new ValidationException("expert", "Expert not found.");
        }

        expert.Bio = request.Bio?.Trim() ?? string.Empty;
        expert.Specialization = request.Specialization?.Trim() ?? string.Empty;
        expert.Rating = request.Rating;
        expert.IsVerified = request.IsVerified;

        await _guidedAdminRepository.UpdateExpertAsync(expert, cancellationToken);
        _logger.LogInformation("Updated guided expert {ExpertId}", expert.UserId);

        return new UpdateExpertResult(
            expert.UserId,
            expert.Bio,
            expert.Specialization,
            expert.Rating,
            expert.IsVerified);
    }
}
