namespace Business.Dtos;

public class RegisterDto
{  
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Country { get; set; }
    public required string Department { get; set; }
    public required string Password { get; set; }
}

