namespace DataAccess.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly AppDbContext _appDbContext;

    public PlanRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<string> AddPlan(Plan plan)
    {
        await _appDbContext.Plans.AddAsync(plan);
        await _appDbContext.SaveChangesAsync();
        return plan.Id;
    }

    public async Task<bool> RemovePlan(Plan plan)
    {
        _appDbContext.Plans.Remove(plan);
        return await _appDbContext.SaveChangesAsync() > 0;
    }

    public async Task<Plan?> GetPlanById(string planId)
    {
        return await _appDbContext.Plans.FindAsync(planId);
    }

    public async Task<bool> UpdatePlan(Plan plan)
    {
        _appDbContext.Plans.Update(plan);
        return await _appDbContext.SaveChangesAsync() > 0;
    }
}