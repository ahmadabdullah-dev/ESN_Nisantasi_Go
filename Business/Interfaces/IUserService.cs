namespace Business.Interfaces;
public interface IUserService
{
    Task<Result<UserDto>> GetCurrentAsync();
}
