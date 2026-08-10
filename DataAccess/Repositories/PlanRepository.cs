using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

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
    public async Task<bool> IsParticipantAsync(string planId, string userId, CancellationToken ct = default)
    {
        return await _appDbContext.PlanParticipants
            .AsNoTracking()
            .AnyAsync(pp => pp.PlanId == planId && pp.ParticipantId == userId, ct);
    }
    public async Task<bool> LeavePlanAsync(PlanParticipant plan, CancellationToken ct = default)
    {
        _appDbContext.PlanParticipants.Remove(plan);
        return await _appDbContext.SaveChangesAsync(ct) > 0;    
    }
    public async Task<PlanParticipant?> GetPlanParticipantAsync(string userId, string planId, CancellationToken ct = default)
    {
       return await _appDbContext.PlanParticipants
                   .Include(x => x.Plan)
            .FirstOrDefaultAsync(x => x.ParticipantId == userId && x.PlanId == planId, ct);
    }
    public async Task<bool> IsPlanParticipatedAsync(string planId, string userId, CancellationToken ct = default)
    {
        return await _appDbContext.PlanParticipants.AnyAsync(x => x.PlanId == planId && x.ParticipantId == userId,ct);
    }
    public async Task<PagedList<PlanProjection>> GetUserPlansAsync(string userId, PaginationParams p, CancellationToken ct = default)
    {
        var query = _appDbContext.PlanParticipants
            .Where(x => x.ParticipantId == userId)
            .OrderByDescending(x => x.JoinedAt)
            .AsNoTracking()
            .Select(x => new PlanProjection
            {
                Id = x.Plan.Id,    
                Title = x.Plan.Title,
                CreatorUserName = x.Plan.Creator.UserName!,
                Description = x.Plan.Description,
                LocationName = x.Plan.LocationName,
                PlannedAt = x.Plan.PlannedAt,
            });    
      // Console.WriteLine(query);

        return await PagedList<PlanProjection>.CreateAsync(query,p.Page,p.PageSize, ct);
    }
}