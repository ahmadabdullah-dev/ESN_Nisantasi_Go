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
            await JoinPlanAsync(plan.Id, ct);
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
      
        if (currentUserId == null)
            return Result<string>.Failure("Unauthorized perform", 401);
      
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
    public async Task<Result<string>> DeletePlanByIdAsync(string id, CancellationToken ct)
    {
        var currentUserId = _userService.GetCurrentUserId();

        if (currentUserId == null)
            return Result<string>.Failure("Unauthorized perform", 401);

        var plan = await _planRepository.GetPlanEntityByIdAsync(id, ct);
       
        if (plan == null)
            return Result<string>.Failure("Plan not found", 404);
       
        if (plan.Creator.Id != currentUserId)
            return Result<string>.Failure("Plan can only be deleted by the creator", 403);
        try
        {
             await LeavePlanAsync(plan.Id, ct);
             await _planRepository.RemovePlanAsync(plan,ct);
             return Result<string>.Success("Plan deleted successfully");
        }
        catch
        {         
            return Result<string>.Failure("Unexpected errror happened", 400);
        }
    }
    public async Task<Result<string>> JoinPlanAsync(string planId, CancellationToken ct)
    {
        var currentUserId = _userService.GetCurrentUserId();
       
        if (currentUserId == null)
            return Result<string>.Failure("Unauthorized perform", 401);

        var plan = await _planRepository.GetPlanEntityByIdAsync(planId, ct);
        
        if (plan == null)
            return Result<string>.Failure("Plan not found", 404);

        var alreadyJoined = await _planRepository.IsParticipantAsync(planId, currentUserId, ct);

        if (alreadyJoined)
           return Result<string>.Failure("Already joined to this plan", 409);

        var ppEntity = new PlanParticipant()
        {
            PlanId = planId,
            ParticipantId = currentUserId,
            JoinedAt = DateTime.UtcNow,
        };
        var ppId = await _planRepository.JoinPlanAsync(ppEntity, ct);

        return ppId == null
        ? Result<string>.Failure("Unexpected error happened", 400)
        : Result<string>.Success($"Joined to {plan.Title} succeessfully");
        
    }
    public async Task<Result<string>> LeavePlanAsync(string planId, CancellationToken ct)
    {
        var currentUserId = _userService.GetCurrentUserId();
        if (currentUserId == null)
            return Result<string>.Failure("Unauthorized", 401);

        var pp = await _planRepository.GetPlanParticipantAsync(currentUserId, planId, ct);
        if (pp == null)
            return Result<string>.Failure("No participation found", 404);

        try
        {
            await _planRepository.LeavePlanAsync(pp, ct);

            if (pp.Plan.CreatorId == currentUserId)
                await DeletePlanByIdAsync(pp.PlanId, ct);

            return Result<string>.Success("Left successfully");
        }
        catch 
        {
            return Result<string>.Failure("Unexpected error happened", 400);
        }
    }
    public async Task<Result<bool>> IsPlanParticipatedAsync(string planId, CancellationToken ct)
    {
        var currentUserId = _userService.GetCurrentUserId();

        if (currentUserId == null)
            return Result<bool>.Failure("Unauthorized", 403);

        var isParticipated = await _planRepository.IsPlanParticipatedAsync(planId, currentUserId, ct);

        return Result<bool>.Success(isParticipated);
    }
    public async Task<Result<PagedList<PlanDto>>> GetCurrentUserPlansAsync(PaginationParams p, CancellationToken ct) 
    {
        var currentUserId = _userService.GetCurrentUserId();
        if (currentUserId == null)
            return Result<PagedList<PlanDto>>.Failure("Unauthorized", 401);

        var plans = await _planRepository.GetUserPlansAsync(currentUserId, p, ct);
        var dtos = new PagedList<PlanDto>
        {
            Items = plans.Items.Select(x => new PlanDto
            {
                Id = x.Id,
                CreatorUserName = x.CreatorUserName,
                Description = x.Description,
                Title = x.Title,
                LocationName = x.LocationName,
                PlannedAt = x.PlannedAt,
            }).ToList(),
            CurrentPage = plans.CurrentPage,
            TotalCount = plans.TotalCount,
            TotalPages = plans.TotalPages,
        };
        return Result<PagedList<PlanDto>>.Success(dtos);
    }
    public async Task<Result<PagedList<PlanDto>>> GetUserPlansAsync(string userId, PaginationParams p, CancellationToken ct)
    {
     
        var plans = await _planRepository.GetUserPlansAsync(userId, p, ct);
      
        var dtos = new PagedList<PlanDto>
        {
            Items = plans.Items.Select(x => new PlanDto
            {
                Id = x.Id,
                CreatorUserName = x.CreatorUserName,
                Description = x.Description,
                Title = x.Title,
                LocationName = x.LocationName,
                PlannedAt = x.PlannedAt,
            }).ToList(),
            CurrentPage = plans.CurrentPage,
            TotalCount = plans.TotalCount,
            TotalPages = plans.TotalPages,
        };
        return Result<PagedList<PlanDto>>.Success(dtos);
    }

}
