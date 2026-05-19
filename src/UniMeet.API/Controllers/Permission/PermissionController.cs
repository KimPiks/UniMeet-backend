using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Contracts.Permissions.Permissions;
using ModularSystem.Contracts.Permissions.Permissions.AddPermission;
using ModularSystem.Contracts.Permissions.Permissions.GetPermissionsForGroup;
using ModularSystem.Contracts.Permissions.Permissions.RemovePermission;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using ModularSystem;

namespace UniMeet.API.Controllers.Permission;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
[ActiveUser]
public class PermissionController(IModuleRequestDispatcher mediator) : ControllerBase
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