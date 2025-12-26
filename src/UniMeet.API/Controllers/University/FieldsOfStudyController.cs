using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.FieldsOfStudy;
using UniMeet.UniversityModule.Application.FieldsOfStudy.AddFieldOfStudy;
using UniMeet.UniversityModule.Application.FieldsOfStudy.DeleteFieldOfStudy;
using UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldOfStudyById;
using UniMeet.UniversityModule.Application.FieldsOfStudy.UpdateFieldOfStudy;

namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("[controller]")]
[ActiveUser]
[Authorize]
public class FieldsOfStudyController(IMediator mediator) : ControllerBase
{
    [HttpGet("{fieldOfStudyId:int}")]
    public async Task<IActionResult> GetFieldOfStudy([FromRoute] int fieldOfStudyId)
    {
        var query = new GetFieldOfStudyByIdQuery(fieldOfStudyId);
        var fieldOfStudy = await mediator.SendAsync(query);
        return Ok(ApiResponse<FieldOfStudyDto?>.Ok(fieldOfStudy, "Field of study retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> CreateFieldOfStudy([FromBody] FieldOfStudyCreateRequest request)
    {
        var command = new AddFieldOfStudyCommand(request.DepartmentId, request.FieldOfStudyName);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Field of study added successfully"));
    }
    
    [HttpDelete("{fieldOfStudyId:int}")]
    public async Task<IActionResult> DeleteFieldOfStudy([FromRoute] int fieldOfStudyId)
    {
        var command = new DeleteFieldOfStudyCommand(fieldOfStudyId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Field of study deleted successfully"));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFieldOfStudy([FromBody] FieldOfStudyUpdateRequest request)
    {
        var command = new UpdateFieldOfStudyCommand(request.FieldOfStudyId, request.FieldOfStudyName);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Field of study updated successfully"));
    }
}