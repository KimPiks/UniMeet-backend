using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Contracts.Permissions.Groups;
using ModularSystem.Contracts.Permissions.Groups.AddGroup;
using ModularSystem.Contracts.Permissions.Groups.GetAllGroups;
using ModularSystem.Contracts.Permissions.Groups.GetGroupByName;
using ModularSystem.Contracts.Permissions.Groups.RemoveGroup;
using ModularSystem.Contracts.Permissions.Groups.UpdateGroup;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using ModularSystem;

namespace UniMeet.API.Controllers.Group;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
[ActiveUser]
public class GroupController(IModuleRequestDispatcher mediator) : ControllerBase
{
    [HttpGet]
    [Permission("PermissionsModule.GetGroups")]
    public async Task<IActionResult> GetGroups([FromQuery] int offset = 0, [FromQuery] int limit = 100)
    {
        var query = new GetAllGroupsQuery(offset, limit);
        var groups = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<GroupDto>>.Ok(groups, "Groups retrieved successfully"));
    }
    
    [HttpGet]
    [Permission("PermissionsModule.GetGroups")]
    public async Task<IActionResult> GetGroup([FromQuery] string groupName)
    {
        var query = new GetGroupByNameQuery(groupName);
        var group = await mediator.SendAsync(query);
        return Ok(ApiResponse<GroupDto>.Ok(group, "Group retrieved successfully"));
    }
    
    [HttpPost]
    [Permission("PermissionsModule.AddGroup")]
    public async Task<IActionResult> AddGroup([FromBody] AddGroupRequest request)
    {
        var command = new AddGroupCommand(request.Name);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "Group added successfully"));
    }

    [HttpDelete]
    [Permission("PermissionsModule.DeleteGroup")]
    public async Task<IActionResult> DeleteGroup([FromBody] DeleteGroupRequest request)
    {
        var command = new RemoveGroupCommand(request.GroupId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "Group deleted successfully"));
    }

    [HttpPut]
    [Permission("PermissionsModule.UpdateGroup")]
    public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroupRequest request)
    {
        var command = new UpdateGroupCommand(request.GroupId, request.Name);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "Group updated successfully"));
    }
}