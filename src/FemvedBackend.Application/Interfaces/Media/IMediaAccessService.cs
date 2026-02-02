using FemvedBackend.Application.Media;

namespace FemvedBackend.Application.Interfaces.Media;

/// <summary>
/// Provides access to media resources.
/// </summary>
public interface IMediaAccessService
{
    Task<MediaAccessResult> GetAccessAsync(MediaAccessRequest request, CancellationToken cancellationToken = default);
}
