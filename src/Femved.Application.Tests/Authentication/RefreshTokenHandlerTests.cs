using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.UseCases.Authentication;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Femved.Application.Tests.Authentication;

public sealed class RefreshTokenHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsNewTokens()
    {
        var tokenService = new Mock<ITokenService>();

        var tokens = new FemvedBackend.Application.Identity.AuthTokenResult(
            "access",
            DateTimeOffset.UtcNow.AddMinutes(15),
            "refresh",
            DateTimeOffset.UtcNow.AddDays(7));

        tokenService
            .Setup(service => service.RefreshAsync("refresh", It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokens);

        var handler = new RefreshTokenHandler(tokenService.Object, NullLogger<RefreshTokenHandler>.Instance);

        var result = await handler.HandleAsync(new RefreshTokenRequest("refresh"));

        Assert.Equal(tokens, result.Tokens);
    }
}
