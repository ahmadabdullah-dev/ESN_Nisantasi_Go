using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities;

public class AppUser : IdentityUser
{
    public string? ProfilePhotoUrl { get; set; }
    public string? ProfilePhotoPublicId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Country { get; set; }
    public required string Department { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
