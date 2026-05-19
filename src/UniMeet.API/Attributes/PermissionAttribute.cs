using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModularSystem;
using ModularSystem.Contracts.Permissions.Permissions.GetPermissionsForGroup;
using ModularSystem.Contracts.User.Users.GetUserAccessInfo;
using UniMeet.API.Responses;

namespace UniMeet.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class PermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _permissionName;

    public PermissionAttribute(string permissionName)
    {
        _permissionName = permissionName;
    }
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user == null || !user.Identity.IsAuthenticated)
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(
                ApiResponse<string>.Fail(null, "Unauthorized")
            );
            return;
        }
        
        var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid userId;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out userId))
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(
                ApiResponse<string>.Fail(null, "Unauthorized")
            );
            return;
        }
        
        if (user.FindFirst("exp") != null)
        {
            var expString = user.FindFirst("exp")?.Value;
            if (long.TryParse(expString, out long exp))
            {
                var expDate = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                if (expDate < DateTime.UtcNow)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(
                        ApiResponse<string>.Fail(null, "Unauthorized")
                    );
                    return;
                }
            }
        }
        
        var dispatcher = context.HttpContext.RequestServices.GetRequiredService<IModuleRequestDispatcher>();
        var dbUser = await dispatcher.SendAsync(new GetUserAccessInfoQuery(userId));
        if (dbUser == null)
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(
                ApiResponse<string>.Fail(null, "Unauthorized")
            );
            return;
        }
        
        var permissions = await dispatcher.SendAsync(new GetPermissionsForGroupQuery(dbUser.GroupId));
        if (!permissions.Any(p => p.PermissionName == _permissionName))
        {
            context.HttpContext.Response.StatusCode = 403;
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(
                ApiResponse<string>.Fail(null, "Forbidden")
            );
        }
    }
}
