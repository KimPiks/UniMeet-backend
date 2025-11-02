using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Application.Interfaces;

public interface IUniversityService
{
    Task<University?> GetUniversityByIdAsync(int universityId);
    Task<IEnumerable<University>> GetAllUniversitiesAsync();
    Task CreateUniversityAsync(string name, string country, string voivodeship, string city, string address);
    Task DeleteUniversityAsync(int universityId);
    
    Task AddDepartmentAsync(int universityId, string departmentName);
    Task<IEnumerable<Department>> GetDepartmentsByUniversityIdAsync(int universityId);
    Task DeleteDepartmentAsync(int universityId, int departmentId);
    
    Task AddAllowedEmailDomainAsync(int universityId, string domain);
    Task<IEnumerable<AllowedEmailDomain>> GetAllowedEmailDomainsByUniversityIdAsync(int universityId);
    Task DeleteAllowedEmailDomainAsync(int universityId, int domainId);
}