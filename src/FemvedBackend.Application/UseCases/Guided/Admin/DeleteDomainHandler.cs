using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles domain deletion.
/// </summary>
public sealed class DeleteDomainHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<DeleteDomainHandler> _logger;

    public DeleteDomainHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<DeleteDomainHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task HandleAsync(DeleteDomainRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting guided domain {DomainId}", request.DomainId);
        var exists = await _guidedAdminRepository.DomainExistsAsync(request.DomainId, cancellationToken);
        if (!exists)
        {
            throw new ValidationException("domain", "Domain not found.");
        }

        await _guidedAdminRepository.DeleteDomainAsync(request.DomainId, cancellationToken);
        _logger.LogInformation("Deleted guided domain {DomainId}", request.DomainId);
    }
}
