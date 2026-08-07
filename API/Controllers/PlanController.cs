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
    public async Task<IActionResult> GetPlanByIdAsync(string planId)
    {
        var result = await _planService.GetPlanByIdAsync(planId);
        return HandleResult(result);
    }
}