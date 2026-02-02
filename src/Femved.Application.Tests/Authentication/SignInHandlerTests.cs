using FemvedBackend.Application.Identity;
using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.UseCases.Authentication;
using FemvedBackend.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Femved.Application.Tests.Authentication;

public sealed class SignInHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsTokensForValidCredentials()
    {
        var userRepository = new Mock<IUserRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var tokenService = new Mock<ITokenService>();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@femved.com",
            PasswordHash = "hash",
            Country = "US",
            Currency = "USD",
            Role = new Role { Name = RoleNames.User }
        };

        var tokens = new AuthTokenResult(
            "access",
            DateTimeOffset.UtcNow.AddMinutes(15),
            "refresh",
            DateTimeOffset.UtcNow.AddDays(7));

        userRepository
            .Setup(repo => repo.GetByEmailWithRoleAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(hasher => hasher.VerifyPassword("Password123!", "hash"))
            .Returns(true);

        tokenService
            .Setup(service => service.GenerateTokensAsync(
                user.Id,
                user.Email,
                It.IsAny<IReadOnlyCollection<string>>(),
                user.Country,
                user.Currency,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokens);

        var handler = new SignInHandler(
            userRepository.Object,
            passwordHasher.Object,
            tokenService.Object,
            NullLogger<SignInHandler>.Instance);

        var request = new SignInRequest(user.Email, "Password123!");

        var result = await handler.HandleAsync(request);

        Assert.Equal(user.Id, result.UserId);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(tokens, result.Tokens);
    }
}
