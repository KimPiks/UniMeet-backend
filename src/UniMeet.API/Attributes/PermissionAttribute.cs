using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PermissionsModule.Domain.Groups;
using UniMeet.UserModule.Domain.Users;

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
        var groupRepository = context.HttpContext.RequestServices.GetRequiredService<IGroupRepository>();
        var dbUser = await userRepository.GetByIdAsync(userId);
        if (dbUser == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var group = await groupRepository.GetByIdAsync(dbUser.GroupId);
        if (group == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        if (!group.Permissions.Any(p => p.PermissionName == _permissionName))
        {
            context.Result = new ForbidResult();
        }
    }
}