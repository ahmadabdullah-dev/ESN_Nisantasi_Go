namespace Business.Interfaces;

public interface IUserService 
{
    Task<Result<string>> RegisterAdmin(RegisterDto dto);
    Task<Result<string>> RegisterMember(RegisterDto dto);
}
