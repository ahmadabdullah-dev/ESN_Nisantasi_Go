namespace DataAccess.Interfaces;

public interface IPlanRepository
{
    Task<string> AddPlan(Plan plan);
    Task<bool> RemovePlan(Plan plan);
    Task<Plan?> GetPlanById(string planId);
    Task<bool> UpdatePlan(Plan plan);
}