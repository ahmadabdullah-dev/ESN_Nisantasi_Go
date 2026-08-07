using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PlanController: BaseApiController
{
    private readonly IPlanService _planService;
    public PlanController(IPlanService planService)
    {
        _planService = planService;
    }

    [HttpGet("{planId}")]
    public async Task<IActionResult> GetPlanByIdAsync(string planId, CancellationToken ct)
    {
        var result = await _planService.GetPlanByIdAsync(planId,ct);
        return HandleResult(result);
    }
}