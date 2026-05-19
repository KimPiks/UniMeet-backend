using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using ModularSystem;
using ModularSystem.Contracts.University.FieldsOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.AddFieldOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.DeleteFieldOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.GetFieldOfStudyById;
using ModularSystem.Contracts.University.FieldsOfStudy.UpdateFieldOfStudy;

namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("[controller]")]
[ActiveUser]
[Authorize]
public class FieldsOfStudyController(IModuleRequestDispatcher mediator) : ControllerBase
{
    [HttpGet("{fieldOfStudyId:int}")]
    [Permission("UniversityModule.GetFieldOfStudy")]
    public async Task<IActionResult> GetFieldOfStudy([FromRoute] int fieldOfStudyId)
    {
        var query = new GetFieldOfStudyByIdQuery(fieldOfStudyId);
        var fieldOfStudy = await mediator.SendAsync(query);
        return Ok(ApiResponse<FieldOfStudyDto?>.Ok(fieldOfStudy, "Field of study retrieved successfully"));
    }

    [HttpPost]
    [Permission("UniversityModule.AddFieldOfStudy")]
    public async Task<IActionResult> CreateFieldOfStudy([FromBody] FieldOfStudyCreateRequest request)
    {
        var command = new AddFieldOfStudyCommand(request.DepartmentId, request.FieldOfStudyName);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Field of study added successfully"));
    }
    
    [HttpDelete("{fieldOfStudyId:int}")]
    [Permission("UniversityModule.DeleteFieldOfStudy")]
    public async Task<IActionResult> DeleteFieldOfStudy([FromRoute] int fieldOfStudyId)
    {
        var command = new DeleteFieldOfStudyCommand(fieldOfStudyId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Field of study deleted successfully"));
    }

    [HttpPut]
    [Permission("UniversityModule.UpdateFieldOfStudy")]
    public async Task<IActionResult> UpdateFieldOfStudy([FromBody] FieldOfStudyUpdateRequest request)
    {
        var command = new UpdateFieldOfStudyCommand(request.FieldOfStudyId, request.FieldOfStudyName);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Field of study updated successfully"));
    }
}