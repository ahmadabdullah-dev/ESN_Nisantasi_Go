using Microsoft.AspNetCore.Identity;

namespace DataAccess;

public class DataSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DataSeeder(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Seed()
    {
        await SeedRoles();
        await SeedUsers();
    }

    public async Task SeedRoles()
    {
        var roleNames = new[] { "SuperAdmin", "Admin", "Member" };

        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public async Task SeedUsers()
    {
        var users = new List<(AppUser user, string role)>
        {
            (new AppUser
            {
                UserName = "superadmin1",
                Email = "superadmin1@test.com",
                EmailConfirmed = true,
                IsActive = true,
                FirstName = "SuperAdmin",
                LastName = "User",
                Country = "World",
                Department = "IT",
                CreatedAt = DateTime.UtcNow.AddMonths(-13)
            }, "SuperAdmin"),

            (new AppUser
            {
                UserName = "admin1",
                Email = "admin1@test.com",
                EmailConfirmed = true,
                IsActive = true,
                FirstName = "Admin",
                LastName = "User",
                Country = "World",
                Department = "Design",
                CreatedAt = DateTime.UtcNow.AddMonths(-6)
            }, "Admin"),

            (new AppUser
            {
                UserName = "member1",
                Email = "member1@test.com",
                EmailConfirmed = true,
                IsActive = true,
                FirstName = "Member",
                LastName = "User",
                Country = "World",
                Department = "Marketing",
                CreatedAt = DateTime.UtcNow.AddMonths(-2)
            }, "Member"),
        };

        foreach (var (user, role) in users)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email!);
            if (existingUser != null)
                continue;

            var result = await _userManager.CreateAsync(user, "Pa$$w0rd");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to seed user '{user.Email}': {errors}");
            }
        }
    }
}