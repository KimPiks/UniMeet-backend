using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModularSystem.Auth;
using UniMeet.API.Responses;

namespace UniMeet.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class PermissionAttribute(string permissionName) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new JsonResult(ApiResponse<string>.Fail(null, "Unauthorized"));
            return;
        }

        if (!user.Claims.Any(claim => claim.Type == AuthClaimTypes.Permission && claim.Value == permissionName))
        {
            context.HttpContext.Response.StatusCode = 403;
            context.Result = new JsonResult(ApiResponse<string>.Fail(null, "Forbidden"));
        }
    }
}
