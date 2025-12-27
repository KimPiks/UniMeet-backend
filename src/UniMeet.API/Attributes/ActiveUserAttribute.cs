using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UniMeet.API.Responses;
using UniMeet.UserModule.Domain.Users;

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

        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var dbUser = await userRepository.GetByIdAsync(userId);
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