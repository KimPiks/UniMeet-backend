using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using ModularSystem;
using ModularSystem.Contracts.University.AllowedEmailDomains;
using ModularSystem.Contracts.University.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;
using ModularSystem.Contracts.University.Departments;
using ModularSystem.Contracts.University.Departments.GetDepartmentsByUniversityId;
using ModularSystem.Contracts.University.Universities;
using ModularSystem.Contracts.University.Universities.CreateUniversity;
using ModularSystem.Contracts.University.Universities.DeleteUniversity;
using ModularSystem.Contracts.University.Universities.GetAllUniversities;
using ModularSystem.Contracts.University.Universities.GetUniversityById;
using ModularSystem.Contracts.University.Universities.UpdateUniversity;

namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("[controller]")]
[ActiveUser]
[Authorize]
public class UniversitiesController(IModuleRequestDispatcher mediator) : ControllerBase
{
    [HttpGet("{universityId:int}")]
    [Permission("UniversityModule.GetUniversity")]
    public async Task<IActionResult> GetUniversityById([FromRoute] int universityId)
    {
        var query = new GetUniversityByIdQuery(universityId);
        var university = await mediator.SendAsync(query);
        return Ok(ApiResponse<UniversityDto?>.Ok(university, "University retrieved successfully"));
    }
    
    [HttpGet]
    [Permission("UniversityModule.GetAllUniversities")]
    public async Task<IActionResult> GetAllUniversities([FromQuery] int offset = 0, [FromQuery] int limit = 100)
    {
        var query = new GetAllUniversitiesQuery(offset, limit);
        var universities = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<UniversityDto>>.Ok(universities, "Universities retrieved successfully"));
    }
    
    [HttpGet("{universityId:int}/AllowedEmailDomains")]
    [Permission("UniversityModule.GetAllowedDomains")]
    public async Task<IActionResult> GetAllowedEmailDomains([FromRoute] int universityId, [FromQuery] int offset = 0, [FromQuery] int limit = 100)
    {
        var query = new GetAllowedEmailDomainsByUniversityIdQuery(universityId, offset, limit);
        var domains = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<AllowedEmailDomainDto>>.Ok(domains, "Allowed email domains retrieved successfully"));
    }
    
    [HttpGet("{universityId:int}/Departments")]
    [Permission("UniversityModule.GetDepartments")]
    public async Task<IActionResult> GetAllDepartments([FromRoute] int universityId, [FromQuery] int offset = 0, [FromQuery] int limit = 100)
    {
        var query = new GetDepartmentsByUniversityIdQuery(universityId, offset, limit);
        var departments = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<DepartmentDto>>.Ok(departments, "Departments retrieved successfully"));

    }

    [HttpPost]
    [Permission( "UniversityModule.CreateUniversity")]
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
    [Permission("UniversityModule.DeleteUniversity")]
    public async Task<IActionResult> DeleteUniversity([FromRoute] int universityId)
    {
        var command = new DeleteUniversityCommand(universityId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "University deleted successfully"));
    }

    [HttpPut]
    [Permission("UniversityModule.UpdateUniversity")]
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