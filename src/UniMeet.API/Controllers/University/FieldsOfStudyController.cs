using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Mediator;
using UniMeet.UniversityModule.Application.FieldsOfStudy;
using UniMeet.UniversityModule.Application.FieldsOfStudy.AddFieldOfStudy;
using UniMeet.UniversityModule.Application.FieldsOfStudy.DeleteFieldOfStudy;
using UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldOfStudyById;
using UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;
using UniMeet.UniversityModule.Application.FieldsOfStudy.UpdateFieldOfStudy;

namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("[controller]")]
public class FieldsOfStudyController(IMediator mediator) : ControllerBase
{
    [HttpGet("{fieldOfStudyId:int}")]
    public async Task<IActionResult> GetFieldOfStudy([FromRoute] int fieldOfStudyId)
    {
        var query = new GetFieldOfStudyByIdQuery(fieldOfStudyId);
        
        try
        {
            var fieldOfStudy = await mediator.SendAsync(query);
            
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

    [HttpPost]
    public async Task<IActionResult> CreateFieldOfStudy([FromBody] FieldOfStudyCreateRequest request)
    {
        var command = new AddFieldOfStudyCommand(request.DepartmentId, request.FieldOfStudyName);
        
        try
        {
            await mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Field of study added successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
    
    [HttpDelete("{fieldOfStudyId:int}")]
    public async Task<IActionResult> DeleteFieldOfStudy([FromRoute] int fieldOfStudyId)
    {
        var command = new DeleteFieldOfStudyCommand(fieldOfStudyId);
        
        try
        {
            await mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Field of study deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFieldOfStudy([FromBody] FieldOfStudyUpdateRequest request)
    {
        var command = new UpdateFieldOfStudyCommand(request.FieldOfStudyId, request.FieldOfStudyName);
        
        try
        {
            await mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Field of study updated successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}