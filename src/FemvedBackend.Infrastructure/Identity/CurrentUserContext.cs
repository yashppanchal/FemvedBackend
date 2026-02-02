using System.Security.Claims;
using FemvedBackend.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Http;

namespace FemvedBackend.Infrastructure.Identity;

public sealed class CurrentUserContext : ICurrentUserContext, ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var value = Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? Principal?.FindFirstValue("sub");

            return Guid.TryParse(value, out var userId) ? userId : null;
        }
    }

    public string? Email => Principal?.FindFirstValue(ClaimTypes.Email) ?? Principal?.FindFirstValue("email");

    public IReadOnlyCollection<string> Roles => Principal?.FindAll(ClaimTypes.Role)
        .Select(role => role.Value)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray() ?? Array.Empty<string>();

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    private ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;
}
