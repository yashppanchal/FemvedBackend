using System.Security.Cryptography;
using System.Text;
using FemvedBackend.Application.Interfaces.Media;
using FemvedBackend.Application.Media;
using Microsoft.Extensions.Options;

namespace FemvedBackend.Infrastructure.Media;

public sealed class SignedUrlMediaAccessService : IMediaAccessService
{
    private readonly MediaAccessOptions _options;

    public SignedUrlMediaAccessService(IOptions<MediaAccessOptions> options)
    {
        _options = options.Value;
    }

    public Task<MediaAccessResult> GetAccessAsync(MediaAccessRequest request, CancellationToken cancellationToken = default)
    {
        var expiresAt = DateTimeOffset.UtcNow.Add(request.ValidFor);
        var expiresUnix = expiresAt.ToUnixTimeSeconds();
        var resourcePath = request.ResourceKey.TrimStart('/');
        var payload = $"{resourcePath}:{expiresUnix}";
        var signature = ComputeSignature(payload, _options.SigningKey);
        var url = $"{_options.BaseUrl.TrimEnd('/')}/{resourcePath}?expires={expiresUnix}&signature={signature}";

        var result = new MediaAccessResult(url, expiresAt);
        return Task.FromResult(result);
    }

    private static string ComputeSignature(string payload, string signingKey)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signingKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
