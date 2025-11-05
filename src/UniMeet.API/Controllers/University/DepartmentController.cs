using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.API.Controllers.University;

public partial class UniversityController
{
    [HttpPost("{universityId:int}/Department")]
    public async Task<IActionResult> CreateDepartment([FromRoute] int universityId, [FromBody] DepartmentCreateRequest request)
    {
        try
        {
            await _universityService.AddDepartmentAsync(universityId, request.DepartmentName);
            return Ok(ApiResponse<object>.Ok(null!, "Department added successfully"));
        } catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpGet("{universityId:int}/Department")]
    public async Task<IActionResult> GetAllDepartments([FromRoute] int universityId)
    {
        try
        {
            var departments = await _universityService.GetDepartmentsByUniversityIdAsync(universityId);
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
        try
        {
            var departments = await _universityService.GetDepartmentsByUniversityIdAsync(universityId);
            var department = departments.FirstOrDefault(d => d.Id == departmentId);
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
        try
        {
            await _universityService.DeleteDepartmentAsync(universityId, departmentId);
            return Ok(ApiResponse<object>.Ok(null!, "Department deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}