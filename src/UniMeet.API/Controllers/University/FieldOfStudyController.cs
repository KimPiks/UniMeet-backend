using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;

namespace UniMeet.API.Controllers.University;

public partial class UniversityController
{
    [HttpGet("{universityId:int}/Department/{departmentId:int}/FieldOfStudy")]
    public async Task<IActionResult> GetFieldsOfStudy([FromRoute] int universityId, [FromRoute] int departmentId)
    {
        try
        {
            var fieldsOfStudy = await _universityService.GetFieldsOfStudyByDepartmentIdAsync(universityId, departmentId);
            return Ok(ApiResponse<object>.Ok(fieldsOfStudy, "Fields of study retrieved successfully"));
        } 
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPost("{universityId:int}/Department/{departmentId:int}/FieldOfStudy")]
    public async Task<IActionResult> CreateFieldOfStudy([FromRoute] int universityId, [FromRoute] int departmentId, [FromBody] FieldOfStudyCreateRequest request)
    {
        try
        {
            await _universityService.AddFieldOfStudyAsync(universityId, departmentId, request.FieldOfStudyName);
            return Ok(ApiResponse<object>.Ok(null!, "Field of study added successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
    
    [HttpDelete("{universityId:int}/Department/{departmentId:int}/FieldOfStudy/{fieldOfStudyId:int}")]
    public async Task<IActionResult> DeleteFieldOfStudy([FromRoute] int universityId, [FromRoute] int departmentId, [FromRoute] int fieldOfStudyId)
    {
        try
        {
            await _universityService.DeleteFieldOfStudyAsync(universityId, departmentId, fieldOfStudyId);
            return Ok(ApiResponse<object>.Ok(null!, "Field of study deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}