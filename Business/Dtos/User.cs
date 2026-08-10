namespace Business.Dtos;

public class UserDto
{
    public string? ProfilePhotoPublicId { get; set; }
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Department { get; set; } = null!;
    public bool IsActive { get; set; }
    public string Role { get; set; } = null!;
}
