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

    [HttpGet("{planId}")]
    public async Task<ActionResult> GetPlanById(string planId, CancellationToken ct)
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
}