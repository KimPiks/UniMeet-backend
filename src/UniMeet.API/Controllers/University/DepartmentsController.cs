using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using ModularSystem;
using ModularSystem.Contracts.University.Departments;
using ModularSystem.Contracts.University.Departments.AddDepartment;
using ModularSystem.Contracts.University.Departments.DeleteDepartment;
using ModularSystem.Contracts.University.Departments.GetDepartmentById;
using ModularSystem.Contracts.University.Departments.GetDepartmentsByUniversityId;
using ModularSystem.Contracts.University.Departments.UpdateDepartment;
using ModularSystem.Contracts.University.FieldsOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;

namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("[controller]")]
[ActiveUser]
[Authorize]
public class DepartmentsController(IModuleRequestDispatcher mediator) : ControllerBase
{
    [HttpPost]
    [Permission("UniversityModule.AddDepartment")]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateRequest request)
    {
        var command = new AddDepartmentCommand(request.UniversityId, request.DepartmentName);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Department added successfully"));
    }
    
    [HttpGet("{departmentId:int}")]
    [Permission("UniversityModule.GetDepartment")]
    public async Task<IActionResult> GetDepartmentById([FromRoute] int departmentId)
    {
        var query = new GetDepartmentByIdQuery(departmentId);
        var department = await mediator.SendAsync(query);     
        return Ok(ApiResponse<DepartmentDto?>.Ok(department, "Department retrieved successfully"));
    }
    
    [HttpGet("{departmentId:int}/FieldsOfStudy")]
    [Permission("UniversityModule.GetFieldsOfStudy")]
    public async Task<IActionResult> GetFieldsOfStudy([FromRoute] int departmentId, [FromQuery] int offset = 0, [FromQuery] int limit = 100)
    {
        var query = new GetFieldsOfStudyByDepartmentIdQuery(departmentId, offset, limit);
        var fieldsOfStudy = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<FieldOfStudyDto>>.Ok(fieldsOfStudy, "Fields of study retrieved successfully"));
    }

    [HttpDelete("{departmentId:int}")]
    [Permission("UniversityModule.DeleteDepartment")]
    public async Task<IActionResult> DeleteDepartment([FromRoute] int departmentId)
    {
        var command = new DeleteDepartmentCommand(departmentId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Department deleted successfully"));
    }

    [HttpPut]
    [Permission("UniversityModule.UpdateDepartment")]
    public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentUpdateRequest request)
    {
        var command = new UpdateDepartmentCommand(request.DepartmentId, request.DepartmentName);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Department updated successfully"));
    }
}