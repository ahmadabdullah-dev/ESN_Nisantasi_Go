namespace DataAccess.Projections;

public class PlanProjection
{
    public required string Id { get; set; }
    public required string CreatorUserName { get; set; }
    public required string Title { get; set; }
    public required string LocationName { get; set; }
    public string? Description { get; set; }
    public required DateTime PlannedAt { get; set; }
}
