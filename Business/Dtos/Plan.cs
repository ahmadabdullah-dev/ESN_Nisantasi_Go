namespace Business.Dtos;

public class PlanDto
{
    public required string Id { get; set; }
    public required string CreatorUserName { get; set; }
    public required string Title { get; set; }
    public required string LocationName { get; set; }
    public string? Description { get; set; }
    public required DateTime PlannedAt { get; set; }
}
public class CreatePlanDto
{
    public required string Title { get; set; }
    public required string LocationName { get; set; }
    public string? Description { get; set; }
    public required DateTime PlannedAt { get; set; }
}
public class UpdatePlanDto
{
    public required string PlanId { get; set; }
    public string? Title { get; set; }
    public string? LocationName { get; set; }
    public string? Description { get; set; }
    public DateTime? PlannedAt { get; set; }
}
