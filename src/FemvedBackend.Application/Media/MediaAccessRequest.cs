namespace FemvedBackend.Application.Media;

/// <summary>
/// Represents a request for media access.
/// </summary>
public sealed record MediaAccessRequest(
    string ResourceKey,
    TimeSpan ValidFor);
