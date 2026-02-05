using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles program deletion.
/// </summary>
public sealed class DeleteProgramHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<DeleteProgramHandler> _logger;

    public DeleteProgramHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<DeleteProgramHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task HandleAsync(DeleteProgramRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting guided program {ProgramId}", request.ProgramId);
        var exists = await _guidedAdminRepository.ProgramExistsAsync(request.ProgramId, cancellationToken);
        if (!exists)
        {
            throw new ValidationException("program", "Program not found.");
        }

        await _guidedAdminRepository.DeleteProgramAsync(request.ProgramId, cancellationToken);
        _logger.LogInformation("Deleted guided program {ProgramId}", request.ProgramId);
    }
}
