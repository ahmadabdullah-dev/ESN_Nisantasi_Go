using DataAccess.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class PlanController: BaseApiController
{
    private readonly IPlanService _planService;
    public PlanController(IPlanService planService)
    {
        _planService = planService;
    }

    [HttpGet]
    public async Task<ActionResult> GetPlanById([FromQuery] string planId, CancellationToken ct)
    {
        var result = await _planService.GetPlanByIdAsync(planId,ct);
        return HandleResult(result);
    }
    [HttpPost("add")]
    public async Task<ActionResult> AddPlan(CreatePlanDto dto, CancellationToken ct)
    {
        var result = await _planService.AddPlanAsync(dto, ct);
        return HandleResult(result);
    }
    [HttpGet("paged")]
    public async Task<ActionResult> GetPlans([FromQuery] PaginationParams p,CancellationToken ct)
    {
        var result = await _planService.GetPlansAsync(p,ct); 
        return HandleResult(result);
    }
    [HttpPut]
    public async Task<ActionResult> UpdatePlan(UpdatePlanDto dto, CancellationToken ct)
    {
        var result = await _planService.UpdatePlanAsync(dto,ct);
        return HandleResult(result);
    }
    [HttpDelete]
    public async Task<ActionResult> DeletePlan(string id, CancellationToken ct) 
    {
        var result = await _planService.DeletePlanByIdAsync(id, ct);
        return HandleResult(result);
    }
    [HttpPost("join-plan")]
    public async Task<ActionResult> JoinPlan(string id,CancellationToken ct)
    {
        var result = await _planService.JoinPlanAsync(id, ct);
        return HandleResult(result);
    }
}