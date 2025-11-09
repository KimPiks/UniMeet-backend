using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.UniversityModule.Application.DTOs;
using MediatR;
using UniMeet.UniversityModule.Application.Universities.Queries.GetUniversityById;
using UniMeet.UniversityModule.Application.Universities.Queries.GetAllUniversities;
using UniMeet.UniversityModule.Application.Universities.Commands.CreateUniversity;
using UniMeet.UniversityModule.Application.Universities.Commands.DeleteUniversity;
using UniMeet.UniversityModule.Application.Universities.Commands.UpdateUniversity;
//using UniMeet.API.Models.Requests; 
namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("api/[controller]")]
public partial class UniversityController : ControllerBase
{

    private readonly IMediator _mediator;
    public UniversityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{universityId:int}")]
    public async Task<IActionResult> GetUniversityById([FromRoute] int universityId)
    {
        var query = new GetUniversityByIdQuery(universityId);
        var university = await _mediator.Send(query);


        if (university == null)
        {
            return NotFound(ApiResponse<object>.Fail("University not found"));
        }
        return Ok(ApiResponse<UniversityDto>.Ok(university, "University retrieved successfully"));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUniversities()
    {
        var query = new GetAllUniversitiesQuery();

        var universities = await _mediator.Send(query);

        return Ok(ApiResponse<IEnumerable<UniversityDto>>.Ok(universities, "Universities retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUniversity([FromBody] UniversityCreateRequest request)
    {
        var command = new CreateUniversityCommand(
            request.Name,
            request.Country,
            request.Voivodeship,
            request.City,
            request.Address
        );

        await _mediator.Send(command);

        return Ok(ApiResponse<object>.Ok(null!, "University created successfully"));
    }

    [HttpDelete("{universityId:int}")]
    public async Task<IActionResult> DeleteUniversity([FromRoute] int universityId)
        {
            var command = new DeleteUniversityCommand(universityId);
        try
        {
            await _mediator.Send(command);
            
            return Ok(ApiResponse<object>.Ok(null!, "University deleted successfully"));
        }
        catch (ArgumentException e)
        {

            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPatch("{universityId:int}")]
    public async Task<IActionResult> UpdateUniversity([FromRoute] int universityId,
        [FromBody] UniversityUpdateRequest request)
    {
        var command = new UpdateUniversityCommand(
            universityId,       
            request.Name,       
            request.Country,
            request.Voivodeship,
            request.City,
            request.Address
        );
        
        try
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.Ok(null!, "University updated successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}