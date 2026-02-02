using System.Net.Http.Headers;
using System.Net.Http.Json;
using FemvedBackend.Api.Contracts.Authentication;
using FemvedBackend.Api.Contracts.Profile;
using FemvedBackend.Application.Identity;
using FemvedBackend.Domain.Entities;
using FemvedBackend.Domain.Enums;
using FemvedBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FemvedBackend.Api.Tests;

public sealed class AuthenticationFlowTests
{
    [Fact]
    public async Task RegisterLoginAndAccessProfile()
    {
        await using var factory = new ApiTestFactory();
        using var client = factory.CreateClient();

        await SeedUserRoleAsync(factory.Services);

        var registerRequest = new RegisterRequestDto(
            "user@femved.com",
            "Password123!",
            "Test",
            "User",
            "US",
            "USD",
            RoleType.User);

        var registerResponse = await client.PostAsJsonAsync("/auth/register", registerRequest);
        registerResponse.EnsureSuccessStatusCode();

        var registerResult = await registerResponse.Content.ReadFromJsonAsync<RegisterResponseDto>();
        Assert.NotNull(registerResult);
        Assert.Equal(registerRequest.Email, registerResult!.Email);

        var loginRequest = new LoginRequestDto(registerRequest.Email, registerRequest.Password);
        var loginResponse = await client.PostAsJsonAsync("/auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
        Assert.NotNull(loginResult);
        Assert.Equal(registerRequest.Email, loginResult!.Email);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Tokens.AccessToken);

        var profileResponse = await client.GetAsync("/me/profile");
        profileResponse.EnsureSuccessStatusCode();

        var profile = await profileResponse.Content.ReadFromJsonAsync<ProfileResponseDto>();
        Assert.NotNull(profile);
        Assert.Equal(registerRequest.Email, profile!.Email);
        Assert.Equal(RoleNames.User, profile.Role);
    }

    private static async Task SeedUserRoleAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Roles.AnyAsync())
        {
            return;
        }

        dbContext.Roles.Add(new Role
        {
            Id = Guid.NewGuid(),
            Name = RoleNames.User,
            Type = RoleType.User
        });

        await dbContext.SaveChangesAsync();
    }
}
