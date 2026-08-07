using DataAccess.Projections;

namespace DataAccess.Interfaces;

public interface IPlanRepository
{
    Task<string> AddPlanAsync(Plan plan);
    Task<bool> RemovePlanAsync(Plan plan);
    Task<PlanProjection?> GetPlanByIdAsync(string planId);
    Task<bool> UpdatePlanAsync(Plan plan);
    Task<PagedList<Plan>> GetPlansAsync(PaginationParams p);
}