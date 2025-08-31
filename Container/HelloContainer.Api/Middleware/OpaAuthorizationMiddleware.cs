using HelloContainer.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace HelloContainer.Api.Middleware;

public class OpaAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOpaService _opaService;

    public OpaAuthorizationMiddleware(RequestDelegate next, IOpaService opaService)
    {
        _next = next;
        _opaService = opaService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api") || !context.User.Identity?.IsAuthenticated == true)
        {
            await _next(context);
            return;
        }

        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            await _next(context);
            return;
        }

        var (resource, action) = ExtractResourceAndAction(context);
        if (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(action))
        {
            await _next(context);
            return;
        }

        var allowed = await _opaService.AuthorizeAsync(context.User, resource, action);
        if (!allowed)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access denied by policy");
            return;
        }

        await _next(context);
    }

    private (string resource, string action) ExtractResourceAndAction(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
        var method = context.Request.Method.ToUpper();

        var pathSegments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        
        if (pathSegments.Length < 2 || pathSegments[0] != "api")
        {
            return (string.Empty, string.Empty);
        }

        var resource = pathSegments[1].TrimEnd('s');
        var action = DetermineAction(method, pathSegments);

        return (resource, action);
    }

    private string DetermineAction(string method, string[] pathSegments)
    {
        return method switch
        {
            "GET" => "read",
            "POST" => "create",
            "PUT" => "update",
            "PATCH" => "update", 
            "DELETE" => "delete",
            _ => "unknown"
        };
    }
}