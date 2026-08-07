namespace Business.Interfaces;

public interface IAdminService 
{
    Task<Result<string>> RegisterAdmin(RegisterUserDto dto);
    Task<Result<string>> RegisterMember(RegisterUserDto dto);
}
