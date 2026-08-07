using DataAccess.Common;

namespace DataAccess.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly AppDbContext _appDbContext;

    public PlanRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<string> AddPlanAsync(Plan plan)
    {
        await _appDbContext.Plans.AddAsync(plan);
        await _appDbContext.SaveChangesAsync();
        return plan.Id;
    }
    public async Task<bool> RemovePlanAsync(Plan plan)
    {
        _appDbContext.Plans.Remove(plan);
        return await _appDbContext.SaveChangesAsync() > 0;
    }
    public async Task<Plan?> GetPlanByIdAsync(string planId)
    {
        return await _appDbContext.Plans.FindAsync(planId);
    }
    public async Task<bool> UpdatePlanAsync(Plan plan)
    {
        _appDbContext.Plans.Update(plan);
        return await _appDbContext.SaveChangesAsync() > 0;
    }
    public async Task<PagedList<Plan>> GetPlansAsync(PaginationParams p)
    {
        return await PagedList<Plan>.CreateAsync(_appDbContext.Plans, p.Page, p.PageSize);
    }
}