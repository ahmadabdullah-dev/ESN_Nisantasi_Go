namespace Business.Interfaces;
public interface IUserService
{
    string? GetCurrentUserId();
    string? GetCurrentUserRole();
    Task<Result<CurrentUserDto>> GetCurrentAsync();
    Task<Result<UserDto>> GetUserByUserNameAsync(string userName);
}
