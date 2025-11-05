using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Application.Interfaces;

public interface IUniversityService
{
    // University methods
    Task<UniversityDto?> GetUniversityByIdAsync(int universityId);
    Task<IEnumerable<UniversityDto>> GetAllUniversitiesAsync();
    Task CreateUniversityAsync(string name, string country, string voivodeship, string city, string address);
    Task DeleteUniversityAsync(int universityId);
    
    // Department methods
    Task AddDepartmentAsync(int universityId, string departmentName);
    Task<IEnumerable<DepartmentDto>> GetDepartmentsByUniversityIdAsync(int universityId);
    Task DeleteDepartmentAsync(int universityId, int departmentId);
    
    // AllowedEmailDomain methods
    Task AddAllowedEmailDomainAsync(int universityId, string domain);
    Task<IEnumerable<AllowedEmailDomainDto>> GetAllowedEmailDomainsByUniversityIdAsync(int universityId);
    Task DeleteAllowedEmailDomainAsync(int universityId, int domainId);
    
    // FieldOfStudy methods
    Task AddFieldOfStudyAsync(int universityId, int departmentId, string fieldOfStudyName);
    Task<IEnumerable<FieldOfStudyDto>> GetFieldsOfStudyByDepartmentIdAsync(int universityId, int departmentId);
    Task DeleteFieldOfStudyAsync(int universityId, int departmentId, int fieldOfStudyId);
}