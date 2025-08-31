namespace HelloContainer.Api.Services;

public class UserService : IUserService
{
    private readonly Dictionary<string, List<string>> _userMappings = new()
    {
        {
            "fredjiang2@fredadgroup.onmicrosoft.com",
            new List<string> { "Admin" }
        }
    };

    public async Task<List<string>> GetUserRolesByEmailAsync(string email)
    {
        await Task.Delay(1);
        
        if (_userMappings.TryGetValue(email, out var roles))
        {
            return roles;
        }
        
        return new List<string>();
    }
}
