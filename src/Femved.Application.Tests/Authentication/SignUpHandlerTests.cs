using FemvedBackend.Application.Identity;
using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.UseCases.Authentication;
using FemvedBackend.Domain.Entities;
using FemvedBackend.Domain.Enums;
using Moq;
using Xunit;

namespace Femved.Application.Tests.Authentication;

public sealed class SignUpHandlerTests
{
    [Fact]
    public async Task HandleAsync_CreatesUserAndReturnsTokens()
    {
        var userRepository = new Mock<IUserRepository>();
        var roleRepository = new Mock<IRoleRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var tokenService = new Mock<ITokenService>();

        var role = new Role { Id = Guid.NewGuid(), Name = RoleNames.User, Type = RoleType.User };
        var tokens = new AuthTokenResult(
            "access",
            DateTimeOffset.UtcNow.AddMinutes(15),
            "refresh",
            DateTimeOffset.UtcNow.AddDays(7));

        userRepository
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        roleRepository
            .Setup(repo => repo.GetByTypeAsync(RoleType.User, It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        passwordHasher
            .Setup(hasher => hasher.HashPassword(It.IsAny<string>()))
            .Returns("hash");

        tokenService
            .Setup(service => service.GenerateTokensAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<IReadOnlyCollection<string>>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokens);

        var handler = new SignUpHandler(
            userRepository.Object,
            roleRepository.Object,
            passwordHasher.Object,
            tokenService.Object,
            NullLogger<SignUpHandler>.Instance);

        var request = new SignUpRequest(
            "user@femved.com",
            "Password123!",
            "Test",
            "User",
            "US",
            "USD",
            RoleType.User);

        var result = await handler.HandleAsync(request);

        Assert.Equal(request.Email, result.Email);
        Assert.Equal(tokens, result.Tokens);

        userRepository.Verify(repo => repo.AddAsync(
            It.Is<User>(user =>
                user.Email == request.Email &&
                user.PasswordHash == "hash" &&
                user.RoleId == role.Id),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
