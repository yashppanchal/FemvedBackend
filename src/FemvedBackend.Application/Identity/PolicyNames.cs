namespace FemvedBackend.Application.Identity;

/// <summary>
/// Defines policy names used for authorization.
/// </summary>
public static class PolicyNames
{
    public const string AdminOnly = "AdminOnly";
    public const string ExpertOnly = "ExpertOnly";
    public const string UserOnly = "UserOnly";
    public const string CanManageCatalog = "CanManageCatalog";
    public const string CanViewAssignedUsers = "CanViewAssignedUsers";
}
