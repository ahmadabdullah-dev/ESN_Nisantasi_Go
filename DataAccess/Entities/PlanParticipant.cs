namespace DataAccess.Entities;

public class PlanParticipant
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string PlanId { get; set; }
    public required Plan Plan { get; set; }
    public required string ParticipantId { get; set; }
    public required AppUser Participant { get; set; }
    public DateTime JoinedAt { get; set; }
}
