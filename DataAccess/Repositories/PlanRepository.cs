using DataAccess.Common;
using DataAccess.Projections;
using Microsoft.EntityFrameworkCore;

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
    public async Task<PlanProjection?> GetPlanByIdAsync(string planId)
    {
        var query = _appDbContext.Plans
            .Where(x => x.Id == planId)
            .Select(x => new PlanProjection
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                LocationName = x.LocationName,
                PlannedAt = x.PlannedAt,
                CreatorUserName = x.Creator.UserName!
            });
       // Console.WriteLine(query.ToQueryString());

        return await query.SingleOrDefaultAsync();
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