using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.AllowedEmailDomains;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;
using UniMeet.UniversityModule.Application.Departments;
using UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;
using UniMeet.UniversityModule.Application.Universities;
using UniMeet.UniversityModule.Application.Universities.CreateUniversity;
using UniMeet.UniversityModule.Application.Universities.DeleteUniversity;
using UniMeet.UniversityModule.Application.Universities.GetAllUniversities;
using UniMeet.UniversityModule.Application.Universities.GetUniversityById;
using UniMeet.UniversityModule.Application.Universities.UpdateUniversity;

namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("[controller]")]
public class UniversitiesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{universityId:int}")]
    public async Task<IActionResult> GetUniversityById([FromRoute] int universityId)
    {
        var query = new GetUniversityByIdQuery(universityId);
        var university = await mediator.SendAsync(query);
        return Ok(ApiResponse<UniversityDto?>.Ok(university, "University retrieved successfully"));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUniversities()
    {
        var query = new GetAllUniversitiesQuery();
        var universities = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<UniversityDto>>.Ok(universities, "Universities retrieved successfully"));
    }
    
    [HttpGet("{universityId:int}/AllowedEmailDomains")]
    public async Task<IActionResult> GetAllowedEmailDomains([FromRoute] int universityId)
    {
        var query = new GetAllowedEmailDomainsByUniversityIdQuery(universityId);
        var domains = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<AllowedEmailDomainDto>>.Ok(domains, "Allowed email domains retrieved successfully"));
    }
    
    [HttpGet("{universityId:int}/Departments")]
    public async Task<IActionResult> GetAllDepartments([FromRoute] int universityId)
    {
        var query = new GetDepartmentsByUniversityIdQuery(universityId);
        var departments = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<DepartmentDto>>.Ok(departments, "Departments retrieved successfully"));

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

        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "University created successfully"));
    }

    [HttpDelete("{universityId:int}")]
    public async Task<IActionResult> DeleteUniversity([FromRoute] int universityId)
    {
        var command = new DeleteUniversityCommand(universityId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "University deleted successfully"));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUniversity([FromBody] UniversityUpdateRequest request)
    {
        var command = new UpdateUniversityCommand(
            request.UniversityId,       
            request.Name,       
            request.Country,
            request.Voivodeship,
            request.City,
            request.Address
        );
        
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "University updated successfully"));
    }
}