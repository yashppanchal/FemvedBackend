namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents an expert profile.
/// </summary>
public class Expert
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public bool IsVerified { get; set; }
    public ICollection<ExpertProduct> ExpertProducts { get; set; } = new List<ExpertProduct>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
