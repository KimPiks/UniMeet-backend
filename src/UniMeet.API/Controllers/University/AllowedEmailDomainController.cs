using Microsoft.AspNetCore.Mvc;
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
public class AllowedEmailDomainsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAllowedEmailDomain([FromBody] AllowedEmailCreateRequest request)
    {
        var command = new AddAllowedEmailDomainCommand(request.UniversityId, request.Domain);
        
        try
        {
            await mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain added successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpGet("{domainId:int}")]
    public async Task<IActionResult> GetAllowedEmailDomain([FromRoute] int domainId)
    {
        var query = new GetAllowedEmailDomainByIdQuery(domainId);
        
        try
        {
            var domain = await mediator.SendAsync(query);
            
            if (domain == null)
            {
                return NotFound(ApiResponse<object>.Fail("Allowed email domain not found"));
            }
            
            return Ok(ApiResponse<AllowedEmailDomainDto>.Ok(domain, "Allowed email domain retrieved successfully"));
        }
        catch (ArgumentException e) 
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpDelete("{domainId:int}")]
    public async Task<IActionResult> DeleteAllowedEmailDomain([FromRoute] int domainId)
    {
        var command = new DeleteAllowedEmailDomainCommand(domainId);
        
        try
        {
            await mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateAllowedEmailDomain([FromBody] AllowedEmailUpdateRequest request)
    {
        var command = new UpdateAllowedEmailDomainCommand(request.DomainId, request.NewDomain);
        
        try
        {
            await mediator.SendAsync(command);
            return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain updated successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}