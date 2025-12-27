using Microsoft.AspNetCore.Mvc.Filters;
using UniMeet.API.Responses;

namespace UniMeet.API.Attributes;

public class OnlyAnonymousAttribute : Attribute, IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
        {
            context.HttpContext.Response.StatusCode = 403;
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(
                ApiResponse<string>.Fail(null, "This endpoint is for anonymous users only")
                );
        }
        
        return Task.CompletedTask;
    }
}