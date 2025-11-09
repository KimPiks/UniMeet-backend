using MediatR; 
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.AddAllowedEmailDomain;
using UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.DeleteAllowedEmailDomain;
using UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.UpdateAllowedEmailDomain;
using UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Queries.GetAllowedEmailDomainsByUniversityId;
using UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Queries.GetAllowedEmailDomainById; 

namespace UniMeet.API.Controllers.University;

public partial class UniversityController
{
    [HttpPost("{universityId:int}/AllowedEmailDomain")]
    public async Task<IActionResult> CreateAllowedEmailDomain([FromRoute] int universityId, [FromBody] AllowedEmailCreateRequest request)
    {
        var command = new AddAllowedEmailDomainCommand(universityId, request.Domain);
        
        try
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain added successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpGet("{universityId:int}/AllowedEmailDomain")]
    public async Task<IActionResult> GetAllowedEmailDomains([FromRoute] int universityId)
    {
        var query = new GetAllowedEmailDomainsByUniversityIdQuery(universityId);
        
        try
        {
            var domains = await _mediator.Send(query);
            return Ok(ApiResponse<IEnumerable<AllowedEmailDomainDto>>.Ok(domains, "Allowed email domains retrieved successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpGet("{universityId:int}/AllowedEmailDomain/{domainId:int}")]
    public async Task<IActionResult> GetAllowedEmailDomain([FromRoute] int universityId, [FromRoute] int domainId)
    {
        var query = new GetAllowedEmailDomainByIdQuery(universityId, domainId);
        
        try
        {
            var domain = await _mediator.Send(query);
            
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

    [HttpDelete("{universityId:int}/AllowedEmailDomain/{domainId:int}")]
    public async Task<IActionResult> DeleteAllowedEmailDomain([FromRoute] int universityId, [FromRoute] int domainId)
    {
        var command = new DeleteAllowedEmailDomainCommand(universityId, domainId);
        
        try
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPatch("{universityId:int}/AllowedEmailDomain/{domainId:int}")]
    public async Task<IActionResult> UpdateAllowedEmailDomain([FromRoute] int universityId, [FromRoute] int domainId,
        [FromBody] AllowedEmailUpdateRequest request)
    {
        var command = new UpdateAllowedEmailDomainCommand(universityId, domainId, request.Domain);
        
        try
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain updated successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}