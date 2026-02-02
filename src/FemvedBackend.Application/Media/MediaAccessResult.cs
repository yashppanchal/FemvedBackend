namespace FemvedBackend.Application.Media;

/// <summary>
/// Represents a media access response.
/// </summary>
public sealed record MediaAccessResult(
    string Url,
    DateTimeOffset ExpiresAt);
