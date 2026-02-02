using System;
using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Application.Identity;

/// <summary>
/// Maps <see cref="RoleType"/> values to role names stored in the database.
/// </summary>
public static class RoleTypeMappings
{
    public static string ToRoleName(RoleType roleType)
    {
        return roleType switch
        {
            RoleType.User => RoleNames.User,
            RoleType.Experts => RoleNames.Expert,
            RoleType.Admin => RoleNames.Admin,
            _ => throw new ArgumentOutOfRangeException(nameof(roleType), roleType, "Unknown role type.")
        };
    }
}
