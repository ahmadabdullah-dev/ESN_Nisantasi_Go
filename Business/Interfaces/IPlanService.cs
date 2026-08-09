namespace Business.Interfaces;
public interface IPlanService
{
    Task<Result<PlanDto>> GetPlanByIdAsync(string planId, CancellationToken ct);
    Task<Result<string>> AddPlanAsync(CreatePlanDto dto, CancellationToken ct);
    Task<Result<PagedList<PlanDto>>> GetPlansAsync(PaginationParams p, CancellationToken ct);
    Task<Result<string>> UpdatePlanAsync(UpdatePlanDto dto, CancellationToken ct);
    Task<Result<string>> DeletePlanByIdAsync(string id, CancellationToken ct);
    Task<Result<string>> JoinPlanAsync(string planId, CancellationToken ct);
    Task<Result<string>> LeavePlanAsync(string planId, CancellationToken ct);
}
