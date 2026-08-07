using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AdminController : BaseApiController
{
    private readonly IAdminService _userService;
    public AdminController(IAdminService userService)
    {
        _userService = userService;
    }
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("register-admin")]
    public async Task<ActionResult> RegisterAdmin([FromBody] RegisterUserDto dto)
    {
        var result = await _userService.RegisterAdmin(dto);
        return HandleResult(result);
    }
    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpPost("register-member")]
    public async Task<ActionResult> RegisterMember([FromBody] RegisterUserDto dto)
    {
        var result = await _userService.RegisterMember(dto);
        return HandleResult(result);
    }
}
