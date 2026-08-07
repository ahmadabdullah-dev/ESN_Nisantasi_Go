namespace DataAccess.Interfaces;

public interface IPlanRepository
{
    Task<string> AddPlanAsync(Plan plan,CancellationToken ct);
    Task<bool> RemovePlanAsync(Plan plan,CancellationToken ct);
    Task<PlanProjection?> GetPlanByIdAsync(string planId,CancellationToken ct);
    Task<bool> UpdatePlanAsync(Plan plan, CancellationToken ct);
    Task<PagedList<PlanProjection>> GetPlansAsync(PaginationParams p,CancellationToken ct);
}