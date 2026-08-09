using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly AppDbContext _appDbContext;

    public PlanRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<string> AddPlanAsync(Plan plan, CancellationToken ct = default)
    {
        await _appDbContext.Plans.AddAsync(plan);
        await _appDbContext.SaveChangesAsync(ct);
        return plan.Id;
    }
    public async Task<bool> RemovePlanAsync(Plan plan, CancellationToken ct = default)
    {
        _appDbContext.Plans.Remove(plan);
        return await _appDbContext.SaveChangesAsync(ct) > 0;
    }
    public async Task<PlanProjection?> GetPlanByIdAsync(string planId, CancellationToken ct = default)
    {
        var query = _appDbContext.Plans.AsNoTracking()
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

        return await query.SingleOrDefaultAsync(ct);
    }
    public async Task<bool> UpdatePlanAsync(Plan plan, CancellationToken ct = default)
    {
        _appDbContext.Plans.Update(plan);
        return await _appDbContext.SaveChangesAsync(ct) > 0;
    }
    public async Task<PagedList<PlanProjection>> GetPlansAsync(PaginationParams p, CancellationToken ct = default)
    {
        var query = _appDbContext.Plans
            .OrderByDescending(x => x.PlannedAt)
            .AsNoTracking()
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
        return await PagedList<PlanProjection>.CreateAsync(query, p.Page, p.PageSize, ct);
    }
    public async Task<Plan?> GetPlanEntityByIdAsync(string Id, CancellationToken ct = default)
    {
        return await _appDbContext.Plans
            .Include(x => x.Creator)
            .FirstOrDefaultAsync(x => x.Id == Id, ct);
    }
    public async Task<string> JoinPlanAsync(PlanParticipant pp, CancellationToken ct = default)
    {
        await _appDbContext.PlanParticipants.AddAsync(pp);
        await _appDbContext.SaveChangesAsync(ct);
        return pp.Id;
    }
    public async 

}