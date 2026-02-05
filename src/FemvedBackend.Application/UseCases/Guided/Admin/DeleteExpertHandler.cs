using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles expert deletion.
/// </summary>
public sealed class DeleteExpertHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<DeleteExpertHandler> _logger;

    public DeleteExpertHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<DeleteExpertHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task HandleAsync(DeleteExpertRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting guided expert {ExpertId}", request.ExpertId);
        var expert = await _guidedAdminRepository.GetExpertByIdAsync(request.ExpertId, cancellationToken);
        if (expert is null)
        {
            throw new ValidationException("expert", "Expert not found.");
        }

        var hasPrograms = await _guidedAdminRepository.ExpertHasProgramsAsync(request.ExpertId, cancellationToken);
        if (hasPrograms)
        {
            throw new ValidationException("expert", "Expert has existing programs.");
        }

        expert.IsVerified = false;

        await _guidedAdminRepository.UpdateExpertAsync(expert, cancellationToken);
        _logger.LogInformation("Deleted guided expert {ExpertId}", expert.UserId);
    }
}
