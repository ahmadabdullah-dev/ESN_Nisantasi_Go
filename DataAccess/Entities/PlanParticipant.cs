namespace DataAccess.Entities;

public class PlanParticipant
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string PlanId { get; set; }
    public Plan Plan { get; set; } = null!;
    public required string ParticipantId { get; set; }
    public AppUser Participant { get; set; } = null!;
    public DateTime JoinedAt { get; set; }
}
