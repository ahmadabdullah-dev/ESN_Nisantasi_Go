namespace Business.Services;

public class PlanService : IPlanService
{
    private readonly IPlanRepository _planRepository;
    private readonly IUserService _userService;
    public PlanService(IPlanRepository planRepository,
        IUserService userService)
    {
        _planRepository = planRepository;
        _userService = userService;
    }

    public async Task<Result<PlanDto>> GetPlanByIdAsync(string planId, CancellationToken ct)
    {
        var plan = await _planRepository.GetPlanByIdAsync(planId, ct);

        if (plan == null)
            return Result<PlanDto>.Failure("Plan not found", 404);

        var planDto = new PlanDto
        {
            Id = plan.Id,
            CreatorUserName = plan.CreatorUserName,
            Title = plan.Title,
            LocationName = plan.LocationName,
            Description = plan.Description,
            PlannedAt = plan.PlannedAt
        };

        return Result<PlanDto>.Success(planDto);
    }
    public async Task<Result<string>> AddPlanAsync(CreatePlanDto dto, CancellationToken ct)
    {
        var userId = _userService.GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Result<string>.Failure("User is not authenticated", 401);

        var plan = new Plan
        {
            Title = dto.Title,
            LocationName = dto.LocationName,
            Description = dto.Description,
            PlannedAt = dto.PlannedAt,
            CreatorId = userId
        };

        try
        {
            await _planRepository.AddPlanAsync(plan,ct);
            return Result<string>.Success("Plan added succcessfully");
        }
        catch
        {
            return Result<string>.Failure("Unexpected error happened", 500);
        }
    }
   public async Task<Result<PagedList<PlanDto>>> GetPlansAsync(PaginationParams p, CancellationToken ct)
   {
        var plans = await _planRepository.GetPlansAsync(p, ct);
        
        var dtos =  new PagedList<PlanDto>
        {
            Items = plans.Items.Select(x => new PlanDto
            {
                Id = x.Id,
                CreatorUserName = x.CreatorUserName,
                Title = x.Title,
                Description = x.Description,
                LocationName = x.LocationName,
                PlannedAt = x.PlannedAt

            }).ToList(),

            CurrentPage = plans.CurrentPage,
            TotalCount = plans.TotalCount,
            TotalPages = plans.TotalPages,
        };
        return Result<PagedList<PlanDto>>.Success(dtos);
   }
}
