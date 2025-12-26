using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.API.Attributes;

public class ActiveUserAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid userId;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out userId))
        {
            context.Result = new UnauthorizedResult();
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
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }

        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var dbUser = await userRepository.GetByIdAsync(userId);
        if (dbUser == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!dbUser.IsActive)
        {
            context.Result = new ObjectResult("User is not active.") { StatusCode = 403 };
        }
    }
}