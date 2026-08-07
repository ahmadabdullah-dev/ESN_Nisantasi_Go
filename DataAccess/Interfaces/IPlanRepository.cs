namespace DataAccess.Interfaces;

public interface IPlanRepository
{
    Task<string> AddPlanAsync(Plan plan);
    Task<bool> RemovePlanAsync(Plan plan);
    Task<PlanProjection?> GetPlanByIdAsync(string planId,CancellationToken ct);
    Task<bool> UpdatePlanAsync(Plan plan);
    Task<PagedList<PlanProjection>> GetPlansAsync(PaginationParams p,CancellationToken ct);
}