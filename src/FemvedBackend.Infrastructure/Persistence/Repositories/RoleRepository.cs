using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Domain.Entities;
using FemvedBackend.Application.Identity;
using FemvedBackend.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FemvedBackend.Infrastructure.Persistence.Repositories;

public sealed class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _dbContext;

    public RoleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Role?> GetByTypeAsync(RoleType roleType, CancellationToken cancellationToken = default)
    {
        var roleName = RoleTypeMappings.ToRoleName(roleType);

        return _dbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(role => role.Name == roleName, cancellationToken);
    }
}
