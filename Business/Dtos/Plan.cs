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

