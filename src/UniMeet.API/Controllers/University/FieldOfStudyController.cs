using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.UniversityModule.Application.DTOs; 
using UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.AddFieldOfStudy;
using UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.DeleteFieldOfStudy;
using UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.UpdateFieldOfStudy;
using UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Queries.GetFieldsOfStudyByDepartmentId;
using UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Queries.GetFieldOfStudyById;

namespace UniMeet.API.Controllers.University;

public partial class UniversityController 
{
    [HttpGet("{universityId:int}/Department/{departmentId:int}/FieldOfStudy")]
    public async Task<IActionResult> GetFieldsOfStudy([FromRoute] int universityId, [FromRoute] int departmentId)
    {
        var query = new GetFieldsOfStudyByDepartmentIdQuery(universityId, departmentId);
        
        try
        {
            var fieldsOfStudy = await _mediator.SendAsync(query);
            
            return Ok(ApiResponse<IEnumerable<FieldOfStudyDto>>.Ok(fieldsOfStudy, "Fields of study retrieved successfully"));
        } 
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpGet("{universityId:int}/Department/{departmentId:int}/FieldOfStudy/{fieldOfStudyId:int}")]
    public async Task<IActionResult> GetFieldOfStudy([FromRoute] int universityId, [FromRoute] int departmentId,
        [FromRoute] int fieldOfStudyId)
    {
        var query = new GetFieldOfStudyByIdQuery(universityId, departmentId, fieldOfStudyId);
        
        try
        {
            var fieldOfStudy = await _mediator.SendAsync(query);
            
            if (fieldOfStudy == null)
            {
                return NotFound(ApiResponse<object>.Fail("Field of study not found"));
            }
            
            return Ok(ApiResponse<FieldOfStudyDto>.Ok(fieldOfStudy, "Field of study retrieved successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPost("{universityId:int}/Department/{departmentId:int}/FieldOfStudy")]
    public async Task<IActionResult> CreateFieldOfStudy([FromRoute] int universityId, [FromRoute] int departmentId, [FromBody] FieldOfStudyCreateRequest request)
    {
        var command = new AddFieldOfStudyCommand(universityId, departmentId, request.FieldOfStudyName);
        
        try
        {
            await _mediator.SendAsync(command);
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
        var command = new DeleteFieldOfStudyCommand(universityId, departmentId, fieldOfStudyId);
        
        try
        {
            await _mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Field of study deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPatch("{universityId:int}/Department/{departmentId:int}/FieldOfStudy/{fieldOfStudyId:int}")]
    public async Task<IActionResult> UpdateFieldOfStudy([FromRoute] int universityId, [FromRoute] int departmentId,
        [FromRoute] int fieldOfStudyId, [FromBody] FieldOfStudyUpdateRequest request)
    {
        var command = new UpdateFieldOfStudyCommand(universityId, departmentId, fieldOfStudyId, request.FieldOfStudyName);
        
        try
        {
            await _mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Field of study updated successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}