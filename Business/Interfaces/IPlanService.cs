namespace Business.Interfaces;
public interface IPlanService
{
    Task<Result<PlanDto>> GetPlanByIdAsync(string planId, CancellationToken ct);
    Task<Result<string>> AddPlanAsync(CreatePlanDto dto, CancellationToken ct);
    Task<Result<PagedList<PlanDto>>> GetPlansAsync(PaginationParams p, CancellationToken ct);
}
