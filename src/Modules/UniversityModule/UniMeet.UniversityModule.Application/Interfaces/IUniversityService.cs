using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Application.Interfaces;

public interface IUniversityService
{
    Task<UniversityDto?> GetUniversityByIdAsync(int universityId);
    Task<IEnumerable<UniversityDto>> GetAllUniversitiesAsync();
    Task CreateUniversityAsync(string name, string country, string voivodeship, string city, string address);
    Task DeleteUniversityAsync(int universityId);
    
    Task AddDepartmentAsync(int universityId, string departmentName);
    Task<IEnumerable<DepartmentDto>> GetDepartmentsByUniversityIdAsync(int universityId);
    Task DeleteDepartmentAsync(int universityId, int departmentId);
    
    Task AddAllowedEmailDomainAsync(int universityId, string domain);
    Task<IEnumerable<AllowedEmailDomainDto>> GetAllowedEmailDomainsByUniversityIdAsync(int universityId);
    Task DeleteAllowedEmailDomainAsync(int universityId, int domainId);
    
    // TODO: update methods and fields of study
}