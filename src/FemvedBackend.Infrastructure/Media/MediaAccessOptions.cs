namespace FemvedBackend.Infrastructure.Media;

public sealed class MediaAccessOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
}
