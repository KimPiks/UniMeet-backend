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
        
        try
        {
            await mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Department added successfully"));
        } catch (ArgumentException e)
        {

            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
    
    [HttpGet("{departmentId:int}")]
    public async Task<IActionResult> GetDepartmentById([FromRoute] int departmentId)
    {
        var query = new GetDepartmentByIdQuery(departmentId);
        
        try
        {
            var department = await mediator.SendAsync(query);            

            if (department == null)
            {
                return NotFound(ApiResponse<object>.Fail("Department not found"));
            }
            
            return Ok(ApiResponse<DepartmentDto>.Ok(department, "Department retrieved successfully"));
        } catch (ArgumentException e) 
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
    
    [HttpGet("{departmentId:int}/FieldsOfStudy")]
    public async Task<IActionResult> GetFieldsOfStudy([FromRoute] int departmentId)
    {
        var query = new GetFieldsOfStudyByDepartmentIdQuery(departmentId);
        
        try
        {
            var fieldsOfStudy = await mediator.SendAsync(query);
            
            return Ok(ApiResponse<IEnumerable<FieldOfStudyDto>>.Ok(fieldsOfStudy, "Fields of study retrieved successfully"));
        } 
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpDelete("{departmentId:int}")]
    public async Task<IActionResult> DeleteDepartment([FromRoute] int departmentId)
    {
        var command = new DeleteDepartmentCommand(departmentId);
        
        try
        {
            await mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Department deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentUpdateRequest request)
    {
        var command = new UpdateDepartmentCommand(request.DepartmentId, request.DepartmentName);
        
        try
        {
            await mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Department updated successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}