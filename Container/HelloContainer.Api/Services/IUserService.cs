namespace HelloContainer.Api.Services;

public interface IUserService
{
    Task<List<string>> GetUserRolesByEmailAsync(string email);
}
