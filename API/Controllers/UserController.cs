using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [Authorize]
    [HttpGet("current")]
    public async Task<ActionResult> GetCurrent()
    {
        var result = await _userService.GetCurrentAsync();
        return HandleResult(result);
    }
}
