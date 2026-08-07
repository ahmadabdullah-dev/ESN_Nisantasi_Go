namespace Business.Services;
public class PlanService : IPlanService
{
    private readonly IPlanRepository _planRepository;
    public PlanService(IPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }

    public async Task<Result<PlanDto>> GetPlanByIdAsync(string planId)
    {
        var plan = await _planRepository.GetPlanByIdAsync(planId);
        
        if(plan == null) 
            return Result<PlanDto>.Failure("Plan not found", 404);
     
        var planDto = new PlanDto
        {
            Id = plan.Id,
            CreatorUserName = plan.Creator.UserName!,
            Title = plan.Title,
            LocationName = plan.LocationName,
            Description = plan.Description,
            PlannedAt = plan.PlannedAt
        };

        return Result<PlanDto>.Success(planDto);
    }
}
