using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Mediator;
using UniMeet.UniversityModule.Application.AllowedEmailDomains;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.AddAllowedEmailDomain;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.DeleteAllowedEmailDomain;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainById;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.UpdateAllowedEmailDomain;

namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("[controller]")]
[ActiveUser]
[Authorize]
public class AllowedEmailDomainsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAllowedEmailDomain([FromBody] AllowedEmailCreateRequest request)
    {
        var command = new AddAllowedEmailDomainCommand(request.UniversityId, request.Domain);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain added successfully"));
    }

    [HttpGet("{domainId:int}")]
    public async Task<IActionResult> GetAllowedEmailDomain([FromRoute] int domainId)
    {
        var query = new GetAllowedEmailDomainByIdQuery(domainId);
        var domain = await mediator.SendAsync(query);
        return Ok(ApiResponse<AllowedEmailDomainDto?>.Ok(domain, "Allowed email domain retrieved successfully"));
    }

    [HttpDelete("{domainId:int}")]
    public async Task<IActionResult> DeleteAllowedEmailDomain([FromRoute] int domainId)
    {
        var command = new DeleteAllowedEmailDomainCommand(domainId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain deleted successfully"));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAllowedEmailDomain([FromBody] AllowedEmailUpdateRequest request)
    {
        var command = new UpdateAllowedEmailDomainCommand(request.DomainId, request.NewDomain);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain updated successfully"));
    }
}