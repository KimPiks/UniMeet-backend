using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionsModule.Application.Permissions;
using PermissionsModule.Application.Permissions.AddPermission;
using PermissionsModule.Application.Permissions.GetPermissionsForGroup;
using PermissionsModule.Application.Permissions.RemovePermission;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.Shared.Abstractions;

namespace UniMeet.API.Controllers.Permission;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
[ActiveUser]
public class PermissionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Permission("PermissionsModule.AddPermission")]
    public async Task<IActionResult> AddPermission([FromBody] AddPermissionRequest request)
    {
        var command = new AddPermissionCommand(request.GroupId, request.PermissionName);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "Permission added successfully"));
    }

    [HttpGet]
    [Permission("PermissionsModule.GetPermissions")]
    public async Task<IActionResult> GetPermission([FromQuery] int groupId)
    {
        var query = new GetPermissionsForGroupQuery(groupId);
        var permissions = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<PermissionDto>>.Ok(permissions, "Permissions retrieved successfully"));
    }

    [HttpDelete]
    [Permission("PermissionsModule.DeletePermission")]
    public async Task<IActionResult> DeletePermission([FromBody] DeletePermissionRequest request)
    {
        var command = new RemovePermissionCommand(request.PermissionId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "Permission deleted successfully"));
    }
}