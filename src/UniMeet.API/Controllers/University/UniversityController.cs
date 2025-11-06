using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Interfaces;

namespace UniMeet.API.Controllers.University;

[ApiController]
[Route("api/[controller]")]
public partial class UniversityController : ControllerBase
{
    private readonly IUniversityService _universityService;
    
    public UniversityController(IUniversityService universityService)
    {
        _universityService = universityService;
    }

    [HttpGet("{universityId:int}")]
    public async Task<IActionResult> GetUniversityById([FromRoute] int universityId)
    {
        var university = await _universityService.GetUniversityByIdAsync(universityId);
        if (university == null)
        {
            return NotFound(ApiResponse<object>.Fail("University not found"));
        }
        return Ok(ApiResponse<UniversityDto>.Ok(university, "University retrieved successfully"));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUniversities()
    {
        var universities = await _universityService.GetAllUniversitiesAsync();
        return Ok(ApiResponse<IEnumerable<UniversityDto>>.Ok(universities, "Universities retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUniversity([FromBody] UniversityCreateRequest request)
    {
        await _universityService.CreateUniversityAsync(request.Name, request.Country, request.Voivodeship, request.City, request.Address);
        return Ok(ApiResponse<object>.Ok(null!, "University created successfully"));
    }

    [HttpDelete("{universityId:int}")]
    public async Task<IActionResult> DeleteUniversity([FromRoute] int universityId)
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

    [HttpPatch("{universityId:int}")]
    public async Task<IActionResult> UpdateUniversity([FromRoute] int universityId,
        [FromBody] UniversityUpdateRequest request)
    {
        try
        {
            var university = await _universityService.GetUniversityByIdAsync(universityId);
            if (university == null)
            {
                return NotFound(ApiResponse<object>.Fail("University not found"));
            }

            // Update only the fields that are provided in the request
            var name = request.Name ?? university.Name;
            var country = request.Country ?? university.Country;
            var voivodeship = request.Voivodeship ?? university.Voivodeship;
            var city = request.City ?? university.City;
            var address = request.Address ?? university.Address;

            // Update the university
            await _universityService.UpdateUniversityAsync(universityId, name, country, voivodeship, city, address);

            return Ok(ApiResponse<object>.Ok(null!, "University updated successfully"));
        }
        catch (ArgumentException e)
        {
            return NotFound(ApiResponse<object>.Fail(e.Message));
        }
    }
}