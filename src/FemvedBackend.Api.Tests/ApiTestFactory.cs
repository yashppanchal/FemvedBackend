using FemvedBackend.Application.Identity;
using FemvedBackend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FemvedBackend.Api.Tests;

public sealed class ApiTestFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"FemvedTestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=Test;Username=test;Password=test",
                ["Jwt:Issuer"] = "Femved.Test",
                ["Jwt:Audience"] = "Femved.Test",
                ["Jwt:SigningKey"] = "test-signing-key-1234567890",
                ["Jwt:AccessTokenMinutes"] = "15",
                ["Jwt:RefreshTokenDays"] = "7"
            };

            config.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.RemoveAll(typeof(AppDbContext));

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }
}
