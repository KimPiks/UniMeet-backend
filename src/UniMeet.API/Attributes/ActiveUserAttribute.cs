using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModularSystem.Auth;
using UniMeet.API.Responses;

namespace UniMeet.API.Attributes;

public class ActiveUserAttribute : Attribute, IAuthorizationFilter
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

        var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out _))
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new JsonResult(ApiResponse<string>.Fail(null, "Unauthorized"));
            return;
        }

        var isActive = user.FindFirst(AuthClaimTypes.IsActive)?.Value;
        if (!bool.TryParse(isActive, out var active) || !active)
        {
            context.HttpContext.Response.StatusCode = 403;
            context.Result = new JsonResult(ApiResponse<string>.Fail(null, "User is not active"));
        }
    }
}
