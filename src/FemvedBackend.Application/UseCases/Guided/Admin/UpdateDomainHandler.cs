using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles domain updates.
/// </summary>
public sealed class UpdateDomainHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<UpdateDomainHandler> _logger;

    public UpdateDomainHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<UpdateDomainHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task<UpdateDomainResult> HandleAsync(
        UpdateDomainRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating guided domain {DomainId}", request.DomainId);
        var domain = await _guidedAdminRepository.GetDomainByIdAsync(request.DomainId, cancellationToken);
        if (domain is null)
        {
            throw new ValidationException("domain", "Domain not found.");
        }

        var normalizedName = request.Name.Trim();
        var nameExists = await _guidedAdminRepository.DomainNameExistsAsync(
            normalizedName,
            domain.Id,
            cancellationToken);

        if (nameExists)
        {
            throw new ValidationException("name", "Domain name already exists.");
        }

        domain.Name = normalizedName;
        domain.Description = request.Description?.Trim() ?? string.Empty;
        domain.IsActive = request.IsActive;

        await _guidedAdminRepository.UpdateDomainAsync(domain, cancellationToken);

        _logger.LogInformation("Updated guided domain {DomainId}", domain.Id);

        return new UpdateDomainResult(domain.Id, domain.Name, domain.Description, domain.IsActive);
    }
}
