using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.UniversityModule.Application.Departments.AddDepartment;
using UniMeet.UniversityModule.Application.Departments.DeleteDepartment;
using UniMeet.UniversityModule.Application.Departments.GetDepartmentById;
using UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;
using UniMeet.UniversityModule.Application.Departments.UpdateDepartment;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.API.Controllers.University;

public partial class UniversityController
{
    [HttpPost("{universityId:int}/Department")]
    public async Task<IActionResult> CreateDepartment([FromRoute] int universityId, [FromBody] DepartmentCreateRequest request)
    {
        var command = new AddDepartmentCommand(universityId, request.DepartmentName);
        
        try
        {
            await _mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Department added successfully"));
        } catch (ArgumentException e)
        {

            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpGet("{universityId:int}/Department")]
    public async Task<IActionResult> GetAllDepartments([FromRoute] int universityId)
    {

        var query = new GetDepartmentsByUniversityIdQuery(universityId);
        try
        {
            var departments = await _mediator.SendAsync(query);
            return Ok(ApiResponse<IEnumerable<DepartmentDto>>.Ok(departments, "Departments retrieved successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
    
    [HttpGet("{universityId:int}/Department/{departmentId:int}")]
    public async Task<IActionResult> GetDepartmentById([FromRoute] int universityId, [FromRoute] int departmentId)
    {
        var query = new GetDepartmentByIdQuery(universityId, departmentId);
        
        try
        {
            var department = await _mediator.SendAsync(query);            

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

    [HttpDelete("{universityId:int}/Department/{departmentId:int}")]
    public async Task<IActionResult> DeleteDepartment([FromRoute] int universityId, [FromRoute] int departmentId)
    {
        var command = new DeleteDepartmentCommand(universityId, departmentId);
        
        try
        {
            await _mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Department deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPatch("{universityId:int}/Department/{departmentId:int}")]
    public async Task<IActionResult> UpdateDepartment([FromRoute] int universityId, [FromRoute] int departmentId,
        [FromBody] DepartmentUpdateRequest request)
    {
        var command = new UpdateDepartmentCommand(universityId, departmentId, request.DepartmentName);
        
        try
        {
            await _mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Department updated successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}