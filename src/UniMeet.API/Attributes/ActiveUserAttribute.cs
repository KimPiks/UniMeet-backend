using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModularSystem;
using ModularSystem.Contracts.User.Users.GetUserAccessInfo;
using UniMeet.API.Responses;

namespace UniMeet.API.Attributes;

public class ActiveUserAttribute : Attribute, IAsyncAuthorizationFilter
{
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

        if (!dbUser.IsActive)
        {
            context.HttpContext.Response.StatusCode = 403;
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(
                ApiResponse<string>.Fail(null, "User is not active")
            );
        }
    }
}
