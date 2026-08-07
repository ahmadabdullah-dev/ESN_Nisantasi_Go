namespace DataAccess.Entities;

public class Plan 
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string CreatorId { get; set; }
    public AppUser Creator { get; set; } = null!;
    public required string Title { get; set; }
    public required string LocationName { get; set; }
    public string? Description { get; set; }    
    public required DateTime PlannedAt { get; set; }

    public ICollection<PlanParticipant> Participants { get; set; } = new List<PlanParticipant>();
    public Chat? Chat { get; set; }
}
