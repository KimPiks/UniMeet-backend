using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Mediator;
using UniMeet.UniversityModule.Application.Departments;
using UniMeet.UniversityModule.Application.Departments.AddDepartment;
using UniMeet.UniversityModule.Application.Departments.DeleteDepartment;
using UniMeet.UniversityModule.Application.Departments.GetDepartmentById;
using UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;
using UniMeet.UniversityModule.Application.Departments.UpdateDepartment;
using UniMeet.UniversityModule.Application.FieldsOfStudy;
using UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;

namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("[controller]")]
public class DepartmentsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateRequest request)
    {
        var command = new AddDepartmentCommand(request.UniversityId, request.DepartmentName);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Department added successfully"));
    }
    
    [HttpGet("{departmentId:int}")]
    public async Task<IActionResult> GetDepartmentById([FromRoute] int departmentId)
    {
        var query = new GetDepartmentByIdQuery(departmentId);
        var department = await mediator.SendAsync(query);     
        return Ok(ApiResponse<DepartmentDto?>.Ok(department, "Department retrieved successfully"));
    }
    
    [HttpGet("{departmentId:int}/FieldsOfStudy")]
    public async Task<IActionResult> GetFieldsOfStudy([FromRoute] int departmentId, [FromQuery] int offset = 0, [FromQuery] int limit = 100)
    {
        var query = new GetFieldsOfStudyByDepartmentIdQuery(departmentId, offset, limit);
        var fieldsOfStudy = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<FieldOfStudyDto>>.Ok(fieldsOfStudy, "Fields of study retrieved successfully"));
    }

    [HttpDelete("{departmentId:int}")]
    public async Task<IActionResult> DeleteDepartment([FromRoute] int departmentId)
    {
        var command = new DeleteDepartmentCommand(departmentId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Department deleted successfully"));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentUpdateRequest request)
    {
        var command = new UpdateDepartmentCommand(request.DepartmentId, request.DepartmentName);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Department updated successfully"));
    }
}