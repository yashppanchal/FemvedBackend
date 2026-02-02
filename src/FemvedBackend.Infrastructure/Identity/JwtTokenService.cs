using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FemvedBackend.Application.Identity;
using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FemvedBackend.Infrastructure.Identity;

public sealed class JwtTokenService : ITokenService
{
    private const int RefreshTokenByteSize = 64;
    private const int RefreshTokenHashSize = 32;

    private readonly JwtOptions _jwtOptions;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public JwtTokenService(IOptions<JwtOptions> jwtOptions, IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtOptions = jwtOptions.Value;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthTokenResult> GenerateTokensAsync(
        Guid userId,
        string email,
        IReadOnlyCollection<string> roles,
        string country,
        string currency,
        CancellationToken cancellationToken = default)
    {
        var accessTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);
        var refreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenDays);

        var accessToken = GenerateAccessToken(userId, email, roles, country, currency, accessTokenExpiresAt);
        var (refreshToken, refreshTokenHash) = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = refreshTokenHash,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = refreshTokenExpiresAt
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        return new AuthTokenResult(accessToken, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt);
    }

    public async Task<AuthTokenResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var refreshTokenHash = HashRefreshToken(refreshToken);
        var existingToken = await _refreshTokenRepository.GetByTokenHashAsync(refreshTokenHash, cancellationToken);

        if (existingToken is null || existingToken.IsExpired || existingToken.IsRevoked || existingToken.User is null)
        {
            throw new UnauthorizedAccessException("Refresh token is invalid.");
        }

        var user = existingToken.User;
        var roleName = user.Role?.Name ?? string.Empty;
        var accessTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);
        var refreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenDays);
        var accessToken = GenerateAccessToken(user.Id, user.Email, new[] { roleName }, user.Country, user.Currency, accessTokenExpiresAt);
        var (newRefreshToken, newRefreshTokenHash) = GenerateRefreshToken();

        existingToken.RevokedAt = DateTimeOffset.UtcNow;
        existingToken.ReplacedByTokenHash = newRefreshTokenHash;

        await _refreshTokenRepository.UpdateAsync(existingToken, cancellationToken);

        var newTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = newRefreshTokenHash,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = refreshTokenExpiresAt
        };

        await _refreshTokenRepository.AddAsync(newTokenEntity, cancellationToken);

        return new AuthTokenResult(accessToken, accessTokenExpiresAt, newRefreshToken, refreshTokenExpiresAt);
    }

    public async Task RevokeAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var refreshTokenHash = HashRefreshToken(refreshToken);
        var existingToken = await _refreshTokenRepository.GetByTokenHashAsync(refreshTokenHash, cancellationToken);

        if (existingToken is null || existingToken.IsRevoked)
        {
            return;
        }

        existingToken.RevokedAt = DateTimeOffset.UtcNow;
        await _refreshTokenRepository.UpdateAsync(existingToken, cancellationToken);
    }

    private string GenerateAccessToken(
        Guid userId,
        string email,
        IReadOnlyCollection<string> roles,
        string country,
        string currency,
        DateTimeOffset expiresAt)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new("userId", userId.ToString()),
            new("country", country),
            new("currency", currency)
        };

        foreach (var role in roles.Where(role => !string.IsNullOrWhiteSpace(role)))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("role", role));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static (string Token, string TokenHash) GenerateRefreshToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(RefreshTokenByteSize);
        var token = Convert.ToBase64String(tokenBytes);
        var hash = HashRefreshToken(token);
        return (token, hash);
    }

    private static string HashRefreshToken(string refreshToken)
    {
        var tokenBytes = Encoding.UTF8.GetBytes(refreshToken);
        var hashBytes = SHA256.HashData(tokenBytes);
        return Convert.ToBase64String(hashBytes);
    }
}
