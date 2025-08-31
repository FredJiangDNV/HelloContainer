using System.Security.Claims;
using System.Text.Json;
using Microsoft.Identity.Web;

namespace HelloContainer.Api.Services;

public class OpaService : IOpaService
{
    private readonly HttpClient _httpClient;
    private readonly IUserService _userService;
    private readonly string _policyPath;

    public OpaService(HttpClient httpClient, IUserService userService, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _userService = userService;
        _policyPath = configuration["Opa:PolicyPath"] ?? "/v1/data/container/auth/allow";
    }


    public async Task<bool> AuthorizeAsync(ClaimsPrincipal user, string resource, string action)
    {
        var opaInput = await BuildOpaInputAsync(user, resource, action);
        return await EvaluateAsync(opaInput);
    }

    private async Task<OpaInput> BuildOpaInputAsync(ClaimsPrincipal user, string resource, string action)
    {
        var email = user.GetLoginHint() ?? 
                   user.FindFirst(ClaimTypes.Email)?.Value ?? 
                   user.FindFirst("email")?.Value ?? 
                   user.FindFirst("preferred_username")?.Value ?? 
                   string.Empty;

        var rolesFromClaims = user.FindAll("roles").Select(c => c.Value).ToList();
        if (!rolesFromClaims.Any())
            rolesFromClaims = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        List<string> userRoles;
        if (rolesFromClaims.Any())
            userRoles = rolesFromClaims;
        else
            userRoles = await _userService.GetUserRolesByEmailAsync(email);

        var opaUser = new OpaUser
        {
            Id = user.GetObjectId() ?? email,
            Email = email,
            Name = user.GetDisplayName() ?? user.FindFirst("name")?.Value ?? email,
            Roles = userRoles,
            Groups = user.FindAll("groups").Select(c => c.Value).ToList()
        };

        var opaRequest = new OpaRequest
        {
            Method = "POST",
            Path = $"/api/{resource}s",
            Resource = resource,
            Action = action
        };

        return new OpaInput
        {
            User = opaUser,
            Request = opaRequest
        };
    }

    private async Task<bool> EvaluateAsync(OpaInput input)
    {
        var requestBody = new { input };
        var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_policyPath, content);

        if (!response.IsSuccessStatusCode)
            return false;

        var responseContent = await response.Content.ReadAsStringAsync();
        var opaResponse = JsonSerializer.Deserialize<OpaResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return opaResponse?.Result ?? false;
    }


}