using System.Security.Claims;

namespace HelloContainer.Api.Services;

public interface IOpaService
{
    Task<bool> AuthorizeAsync(ClaimsPrincipal user, string resource, string action);
}

public class OpaInput
{
    public OpaUser User { get; set; } = new();
    public OpaRequest Request { get; set; } = new();
}

public class OpaUser
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> Groups { get; set; } = new();
}

public class OpaRequest
{
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public double? Amount { get; set; }
}

public class OpaResponse
{
    public bool Result { get; set; }
}

