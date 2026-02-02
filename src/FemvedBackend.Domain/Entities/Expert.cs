namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents an expert profile.
/// </summary>
public class Expert : Entity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public ICollection<ExpertProduct> ExpertProducts { get; set; } = new List<ExpertProduct>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
