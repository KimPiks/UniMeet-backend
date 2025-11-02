using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Interfaces;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityService _universityService;
    
    public UniversityController(IUniversityService universityService)
    {
        _universityService = universityService;
    }

    [HttpGet("[action]/{universityId:int}")]
    public async Task<IActionResult> GetById(int universityId)
    {
        var university = await _universityService.GetUniversityByIdAsync(universityId);
        if (university == null)
        {
            return NotFound(ApiResponse<object>.Fail("University not found"));
        }
        return Ok(ApiResponse<UniversityDto>.Ok(university, "University retrieved successfully"));
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        var universities = await _universityService.GetAllUniversitiesAsync();
        return Ok(ApiResponse<IEnumerable<UniversityDto>>.Ok(universities, "Universities retrieved successfully"));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromBody] UniversityCreateRequest request)
    {
        await _universityService.CreateUniversityAsync(request.Name, request.Country, request.Voivodeship, request.City, request.Address);
        return Ok(ApiResponse<object>.Ok(null!, "University created successfully"));
    }

    [HttpDelete("[action]/{universityId:int}")]
    public async Task<IActionResult> Delete(int universityId)
    {
        try
        {
            await _universityService.DeleteUniversityAsync(universityId);
            return Ok(ApiResponse<object>.Ok(null!, "University deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
    
    [HttpPost("Department/[action]")]
    public async Task<IActionResult> Create([FromBody] DepartmentCreateRequest request)
    {
        try
        {
            await _universityService.AddDepartmentAsync(request.UniversityId, request.DepartmentName);
            return Ok(ApiResponse<object>.Ok(null!, "Department added successfully"));
        } catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpGet("Department/GetAll/{universityId:int}")]
    public async Task<IActionResult> GetDepartments(int universityId)
    {
        try
        {
            var departments = await _universityService.GetDepartmentsByUniversityIdAsync(universityId);
            return Ok(ApiResponse<IEnumerable<DepartmentDto>>.Ok(departments, "Departments retrieved successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
    
    [HttpGet("Department/GetById/{universityId:int}/{departmentId:int}")]
    public async Task<IActionResult> GetDepartmentById(int universityId, int departmentId)
    {
        try
        {
            var departments = await _universityService.GetDepartmentsByUniversityIdAsync(universityId);
            var department = departments.FirstOrDefault(d => d.Id == departmentId);
            if (department == null)
            {
                return NotFound(ApiResponse<object>.Fail("Department not found"));
            }
            return Ok(ApiResponse<DepartmentDto>.Ok(department, "Department retrieved successfully"));
        } catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpDelete("Department/Delete/{universityId:int}/{departmentId:int}")]
    public async Task<IActionResult> DeleteDepartment(int universityId, int departmentId)
    {
        try
        {
            await _universityService.DeleteDepartmentAsync(universityId, departmentId);
            return Ok(ApiResponse<object>.Ok(null!, "Department deleted successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpPost("AllowedEmailDomain/[action]")]
    public async Task<IActionResult> Create([FromBody] AllowedEmailCreateRequest request)
    {
        try
        {
            await _universityService.AddAllowedEmailDomainAsync(request.UniversityId, request.Domain);
            return Ok(ApiResponse<object>.Ok(null!, "Allowed email domain added successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }

    [HttpGet("AllowedEmailDomain/GetAll/{universityId:int}")]
    public async Task<IActionResult> GetAllowedEmailDomains(int universityId)
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

    [HttpDelete("AllowedEmailDomain/Delete/{universityId:int}/{domainId:int}")]
    public async Task<IActionResult> DeleteAllowedEmailDomain(int universityId, int domainId)
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
}