using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.API.Controllers.University;

public partial class UniversityController
{
    [HttpPost("{universityId:int}/AllowedEmailDomain")]
    public async Task<IActionResult> CreateAllowedEmailDomain([FromRoute] int universityId, [FromBody] AllowedEmailCreateRequest request)
    {
        try
        {
            await _universityService.AddAllowedEmailDomainAsync(universityId, request.Domain);
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
        try
        {
            var domains = await _universityService.GetAllowedEmailDomainsByUniversityIdAsync(universityId);
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
        try
        {
            var domains = await _universityService.GetAllowedEmailDomainsByUniversityIdAsync(universityId);
            var domain = domains.FirstOrDefault(d => d.Id == domainId);
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
        try
        {
            await _universityService.DeleteAllowedEmailDomainAsync(universityId, domainId);
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
        try
        {
            await _universityService.UpdateAllowedEmailDomainAsync(universityId, domainId,
                request.Domain);
            return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain updated successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}