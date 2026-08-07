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
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("register-admin")]
    public async Task<ActionResult> RegisterAdmin([FromBody] RegisterDto dto)
    {
        var result = await _userService.RegisterAdmin(dto);
        return HandleResult(result);
    }
    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpPost("register-member")]
    public async Task<ActionResult> RegisterMember([FromBody] RegisterDto dto)
    {
        var result = await _userService.RegisterMember(dto);
        return HandleResult(result);
    }
}
