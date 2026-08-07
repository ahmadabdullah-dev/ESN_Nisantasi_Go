using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AdminService> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminService(
        UserManager<AppUser> userManager,
        IEmailService emailService,
        ILogger<AdminService> logger,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task<Result<string>> RegisterAdmin(RegisterUserDto dto) =>
        await RegisterUserWithRole(dto, UserRoles.ADMIN);

    public async Task<Result<string>> RegisterMember(RegisterUserDto dto) =>
        await RegisterUserWithRole(dto, UserRoles.MEMBER);

    private async Task<Result<string>> RegisterUserWithRole(RegisterUserDto dto, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
            return Result<string>.Failure("Invalid role specified.", 400);

        var registerResult = await RegisterUserAsync(dto);
        if (!registerResult.IsSuccess)
            return Result<string>.Failure(registerResult.Error!,400);

        var user = registerResult.Value!;

        var addRoleResult = await _userManager.AddToRoleAsync(user, role);
        if (!addRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return Result<string>.Failure(ServiceHelper.GetFirstError(addRoleResult), 400);
        }

        return Result<string>.Success("Registered successfully.");
    }

    private async Task<Result<AppUser>> RegisterUserAsync(RegisterUserDto dto)
    {
        var username = await GenerateUniqueUsernameAsync(dto.FirstName, dto.LastName);

        var user = new AppUser
        {
            UserName = username,
            Email = dto.Email.Trim().ToLower(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Country = dto.Country,
            Department = dto.Department,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        var registerResult = await _userManager.CreateAsync(user, dto.Password);
        if (!registerResult.Succeeded)
            return Result<AppUser>.Failure(ServiceHelper.GetFirstError(registerResult), 422);

        try
        {
            await _emailService.SendEmailAsync(user.Email, "Welcome", $"Hi {user.FirstName}, welcome to our ESN Nisantasi club!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Welcome email failed for {user.Email}");
        }

        return Result<AppUser>.Success(user);
    }

    private async Task<string> GenerateUniqueUsernameAsync(string firstName, string lastName)
    {
        string baseUsername = $"{firstName.ToLower()}.{lastName.ToLower()}";
        string username = baseUsername;
        int counter = 1;

        while (await _userManager.Users.AnyAsync(u => u.UserName == username))
        {
            username = $"{baseUsername}{counter}";
            counter++;
        }

        return username;
    }
}