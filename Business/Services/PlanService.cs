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
    public async Task<Result<string>> UpdatePlanAsync(UpdatePlanDto dto, CancellationToken ct)
    {
        var currentUserId =  _userService.GetCurrentUserId();

        var plan = await _planRepository.GetPlanEntityByIdAsync(dto.PlanId, ct);

        if (plan == null)
            return Result<string>.Failure("Plan not found", 404);

        if (plan.Creator.Id != currentUserId)
            return Result<string>.Failure("Plan can only be modified by the creator", 403);

        if(!string.IsNullOrEmpty(dto.Title))
            plan.Title = dto.Title.Trim();

        if(!string.IsNullOrEmpty(dto.Description))
            plan.Description = dto.Description.Trim();
        
        if(!string.IsNullOrEmpty(dto.LocationName))
            plan.LocationName = dto.LocationName.Trim();
        
        if((dto.PlannedAt.HasValue))
            plan.PlannedAt = dto.PlannedAt.Value;

        var isUpdated = await _planRepository.UpdatePlanAsync(plan, ct);
        
        if (!isUpdated)
            return Result<string>.Failure("Unexpected errror happened", 400);

        return Result<string>.Success("Plan updated successfully");
    }
}
