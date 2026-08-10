namespace DataAccess.Interfaces;

public interface IPlanRepository
{
    Task<string> AddPlanAsync(Plan plan, CancellationToken ct);
    Task<bool> RemovePlanAsync(Plan plan, CancellationToken ct);
    Task<PlanProjection?> GetPlanByIdAsync(string planId, CancellationToken ct);
    Task<bool> UpdatePlanAsync(Plan plan, CancellationToken ct);
    Task<PagedList<PlanProjection>> GetPlansAsync(PaginationParams p, CancellationToken ct);
    Task<Plan?> GetPlanEntityByIdAsync(string id, CancellationToken ct);
    Task<string> JoinPlanAsync(PlanParticipant pp, CancellationToken ct);
    Task<bool> IsParticipantAsync(string planId, string userId, CancellationToken ct);
    Task<bool> LeavePlanAsync(PlanParticipant pp, CancellationToken ct);
    Task<PlanParticipant?> GetPlanParticipantAsync(string userId,string planId, CancellationToken ct);
    Task<bool> IsPlanParticipatedAsync(string planId, string userId, CancellationToken ct);
    Task<PagedList<PlanProjection>> GetUserPlansAsync(string userId, PaginationParams p, CancellationToken ct);
}