using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
    
[Authorize]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet("current")]
    public async Task<ActionResult> GetCurrent()
    {
        var result = await _userService.GetCurrentAsync();
        return HandleResult(result);
    }
    [HttpGet]
    public async Task<ActionResult> GetByUserName([FromQuery] string userName) 
    {
        var result = await _userService.GetUserByUserNameAsync(userName);
        return HandleResult(result);
    }
}
