namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a base entity with a unique identifier.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; set; }
}
