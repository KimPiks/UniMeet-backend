using Microsoft.AspNetCore.Mvc.Filters;

namespace UniMeet.API.Attributes;

public class OnlyAnonymousAttribute : Attribute, IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
        {
            context.HttpContext.Response.StatusCode = 403;
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new
            {
                Message = "This endpoint is only accessible to anonymous users."
            });
        }
        
        return Task.CompletedTask;
    }
}