using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FemvedBackend.Application.UseCases.Guided.Admin;

/// <summary>
/// Handles domain creation.
/// </summary>
public sealed class CreateDomainHandler
{
    private readonly IGuidedAdminRepository _guidedAdminRepository;
    private readonly ILogger<CreateDomainHandler> _logger;

    public CreateDomainHandler(
        IGuidedAdminRepository guidedAdminRepository,
        ILogger<CreateDomainHandler> logger)
    {
        _guidedAdminRepository = guidedAdminRepository;
        _logger = logger;
    }

    public async Task<CreateDomainResult> HandleAsync(
        CreateDomainRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = request.Name.Trim();
        _logger.LogInformation("Creating guided domain {DomainName}", normalizedName);
        if (await _guidedAdminRepository.DomainNameExistsAsync(normalizedName, null, cancellationToken))
        {
            throw new ValidationException("name", "Domain name already exists.");
        }

        var domain = new global::FemvedBackend.Domain.Entities.Domain
        {
            Name = normalizedName,
            Description = request.Description?.Trim() ?? string.Empty,
            IsActive = request.IsActive
        };

        await _guidedAdminRepository.AddDomainAsync(domain, cancellationToken);

        _logger.LogInformation("Created guided domain {DomainId}", domain.Id);

        return new CreateDomainResult(domain.Id, domain.Name, domain.Description, domain.IsActive);
    }
}
